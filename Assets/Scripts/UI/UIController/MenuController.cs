using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;   
using TMPro;

namespace UIController {
   public class MenuController : MonoBehaviour
   {
      // menus
      [Header("Menus Canvas")]
      public BaseSubMenuController MainMenu;
      public BaseSubMenuController CreateMenu;
      public BaseSubMenuController CreateCardMenu;
      public BaseSubMenuController CategoryColorMenu;
      public BaseSubMenuController CategoryTypeMenu;
      public BaseSubMenuController InitialMenu;
      public PromptMenuController PromptMenu;
      public ProgressMenuController ProgressMenu;

      public ContextualMenuController ContextualMenu;

      public LoadMenuController LoadMenu;

      public ImportCSVMod ImportObjectPrefab;

      // locations
      [Header("Spawn Location")]
      [SerializeField]
      GameObject SpawnLocation;
      
      [Header("Drawing Functions")]
      [SerializeField]
      private DrawController3D Draw3DController;
      public List<Savable> boards;
      public GameObject PenSummonLocation;


      // session
      [Header("Session")]
      [Range(1, 10)]
      public int max_sessions = 1;
      private List<UISession> sessions;
      private int currentSessionID;
      private UISession currentSession;

      // prefab
      [Header("Prefab")]
      public GameObject CardPrefab;
      public GameObject BoardPrefab;

      // prefab renderer
      private Renderer CardPrefabRenderer;
      private Renderer BoardPrefabRenderer;
      
      // save load system
      private SaveLoadSystem saveLoadSys;
      // type of category
      private CategoryTypeEnum categoryType;

      void Awake() {
         sessions = new List<UISession>();

         CardPrefabRenderer = CardPrefab.GetComponent<Renderer>();
         if (CardPrefabRenderer == null) {
            Debug.LogError("Cannot find renderer of card prefab");
         } // end if

         BoardPrefabRenderer = BoardPrefab.GetComponent<Renderer>();
         if (BoardPrefabRenderer == null) {
            Debug.LogError("Cannot find renderer of board prefab");
         } // end if

         if (InitialMenu == null) {
            Debug.LogError("Initial Menu is not set");
         } // end if

         Show(InitialMenu);
         currentSession = new UISession(InitialMenu);
         sessions.Add(currentSession);

         saveLoadSys = new SaveLoadSystem();
         FileManager.Initialize();
         saveLoadSys.Initialize();

      } // end Awake
      public void GoToMenu(BaseSubMenuController des) {
         if (des == null) {
            Debug.LogError("The next menu is null");
            return;
         } // end if
         Hide(currentSession.GetCurrent());
         Show(currentSession.MoveToNewMenu(des));
      } // end SwapMenu

      public void GoPrevMenu() {
         BaseSubMenuController myCurrent = currentSession.GetCurrent();
         BaseSubMenuController des = currentSession.MoveToPrevMenu();
         if (des == null)
            return;

         Hide(myCurrent);
         Show(des);
      } // end GoPrevMenu

      public void PopulateSessionsListView() {
         List<string> sessions = saveLoadSys.GetSessionsList();
         LoadMenu.PopulateSessionList(sessions, Load, DeleteSession);
      } // end PopulateListView

      public void DeleteSession(string sessionPath, UnityAction deleteAction) {
         string session = SaveLoadSystem.GetSessionName(sessionPath);
         ShowPrompt("Do you want to delete session: " + session, delegate {
               ShowProgress("Deleting...", "Session " + session + " is deleted successfully", "The session has not been deleted properly", () => {
                  bool result = saveLoadSys.DeleteSessionFile(sessionPath);
                  if (result)
                     deleteAction();
                  return Task.FromResult(result);
               }); // end ShowProgress
            }); // end ShowPrompt
      } // end DeleteSession

      public void GoForwdMenu() {
         BaseSubMenuController myCurrent = currentSession.GetCurrent();
         BaseSubMenuController des = currentSession.MoveToForwardMenu();
         if (des == null)
            return;
         Hide(myCurrent);   
         Show(des);
      } // end GoForwdMenu
      
      public void SetCategoryType(CategoryType type) {
         categoryType = type.GetValue();
      } // end SetCategoryType

      public void CreateCard(ColorType color) {
         CreateCardInternal(color.GetValue(), SpawnLocation.transform.position, SpawnLocation.transform.rotation);
      } // end CreateCard

      public void SummonPen() {
         Draw3DController.gameObject.transform.position = PenSummonLocation.transform.position;
      } // end SummonPen

      private GameObject CreateCardInternal(Color color, Vector3 position, Quaternion rotation) {
         if (CardPrefabRenderer == null) {
            Debug.LogError("Cannot find renderer of card prefab");
            return null;
         } // end if
         CardPrefabRenderer.material.SetColor("_Color", color);

         GameObject newObj = GameObject.Instantiate(CardPrefab, position, rotation);
         newObj.SetActive(true);
         saveLoadSys.Add(newObj);
         return newObj;
      } // end CreateCard

      public void CreateCategory(ColorType color) {
         if (BoardPrefabRenderer == null) {
            Debug.LogError("Cannot find renderer of board prefab");
            return;
         } // end if
         BoardPrefabRenderer.material.SetColor("_Color", color.GetValue());

         GameObject.Instantiate(BoardPrefab, SpawnLocation.transform.position, SpawnLocation.transform.rotation);
      } // end CreateCategory


      /*
         return the prompt object of the menu. 
         precodnition: menu is not null
      */
      private async void ShowProgress(string prompt, string done_prompt_true, string done_prompt_false, Func<Task<bool>> operation) {
         
         // Set the UI components
         Show(ProgressMenu);
         ProgressMenu.SetPrompt(prompt);
         
         // Run the task
         bool result = await ProgressMenu.ShowOnProgress(operation);

         // if result is true, show the prompt true
         if (result) { // if result is false, show the prompt false
            ProgressMenu.SetPrompt(done_prompt_true);
         } else {
            ProgressMenu.SetPrompt(done_prompt_false);
         } // end else

         // done
         ProgressMenu.ShowDone(delegate {Hide(ProgressMenu);});
      } // end ShowProgress

      public void ShowPrompt(string prompt, UnityAction action) {
         Show(PromptMenu);
         PromptMenu.SetPrompt(prompt);
         PromptMenu.SetButtonActions(delegate {action();}, delegate {PromptMenu.Hide();});
      } // end ShowPrompt

      public void Save() {
         ShowProgress("Saving session...", "Saving is successful", "Saving is not successful", async () => {
            try {
               saveLoadSys.AddExternalStuffs(Draw3DController.GetLines());
               saveLoadSys.AddExternalStuffs(boards);
               await saveLoadSys.SaveOnQuestAsync(saveLoadSys.GetCurrentPath());
               return true;
            } catch (Exception ex) {
               Debug.LogError(ex.Message);
               Debug.LogError(ex.StackTrace);
               return false;
            }
         });
      } // end Save

      public void SaveAs(GameObject obj) {
         TMP_InputField input = obj.GetComponent<TMP_InputField>();
         if (input == null) 
            return;
         SaveAs(input.text);
      } // end SaveAs
      private void SaveAs(string path) {
         ShowProgress("Saving session...", "Saving is successful", "Saving is not successful", async () => {
            try {
               saveLoadSys.AddExternalStuffs(Draw3DController.GetLines());
               saveLoadSys.AddExternalStuffs(boards);
               await saveLoadSys.SaveOnQuestAsync(path, true);
               return true;
            } catch (Exception ex) {
               Debug.LogError(ex.Message);
               Debug.LogError(ex.StackTrace);
               return false;
            }
         });
      } // end SaveAs

      public void Load(string path) {
         ShowPrompt("Do you want to load: " + path, delegate {
            Debug.Log("Do Prompt");
            ShowProgress("Loading...", "File is loaded successfully", "File is not loadded successfully", async () => {
               try {
                  bool result = await PLoadSession(path);
                  return result;
               } catch (Exception ex) {
                  Debug.LogError(ex.Message);
                  Debug.LogError(ex.StackTrace);
                  return false;
               } // end catch
            }); // end ShowProgress
         }); // end ShowPrompt
      } // end Load

      private async Task<bool> PLoadSession(string path) {
         Debug.Log("Start PLoad");
         List<SaveFormat> items = null;
         items = await saveLoadSys.LoadFromQuestAsync(path);
         if (items == null) {
            Debug.LogError("The loaded items is empty");
            return false;
         } // end if

         Delete();
         for (int i = 0; i < items.Count; ++i) {
            SaveFormat item = items[i];
            GameObject obj = null;
            bool addToSaveLoadSys = true;

            if (item.getType() == FormatType.NOTECARD) {
               obj = (GameObject) GameObject.Instantiate(CardPrefab, new Vector3(0, 1, 0), Quaternion.Euler(0, 0, 0));
            } else if (item.getType() == FormatType.DRAWING) {
               obj = Draw3DController.gameObject;
               addToSaveLoadSys = false;
            } else if (item.getType() == FormatType.BOARD) {
               BoardSaveFormat myItem = (BoardSaveFormat) item;
               foreach (Savable b in boards) {
                  BoardSavable board = (BoardSavable) b;
                  if (board.id == myItem.board_no) {
                     obj = board.gameObject;
                     break;
                  } // end if
               } // end foreach
            } else {
            } // end else
            
            if (obj != null) {
               await item.LoadObjectInto(obj);
               if (addToSaveLoadSys)
                  saveLoadSys.Add(obj);
            } // end if
         } // end foreach

         Debug.Log("End PLoad");
         return true;
      } // end PLoadSession
      
      public void Delete() {
         saveLoadSys.AddExternalStuffs(Draw3DController.GetLines());
         saveLoadSys.AddExternalStuffs(boards);
         Draw3DController.ClearAllDrawingsList();
         saveLoadSys.Clear();
      } // end Delete
      
      private void Hide(BaseSubMenuController menu) {
         menu.Hide();
      } // end Hide

      private void Show(BaseSubMenuController menu) {
         menu.Show();
      } // end Show

       public void Import(string path, bool willParse=false) {
         ShowProgress("Importing...", "Created the import file creator successfully", "Failed to import this file", async () => {
            if (!path.EndsWith(".csv")) {
               return false;
            } // end if

            // to be done
            //Debug.Log("my path is " + path);
            string myText = await FileManager.ReadStringFromAsync(path);
            //Debug.Log("My text is: " + myText);
            GameObject importObj = GameObject.Instantiate(ImportObjectPrefab.gameObject, SpawnLocation.transform.position, SpawnLocation.transform.rotation);
            //CreateCardInternal(Color.yellow);
            importObj.GetComponent<ImportCSVMod>().Initialize(CardPrefab, myText, (string title, string text) => {
               GameObject notecard = CreateCardInternal(Color.yellow, importObj.transform.position + Vector3.up * 2, SpawnLocation.transform.rotation);
               //notecard.GetComponentInChildren<NotecardTextEdit>().ChangeText(text); //set text on child component, TextMeshPro, of Notecard object
               RelativeDisplay rd = notecard.GetComponent<RelativeDisplay>();
               rd.Title = title;
               rd.LongInfo = text;
               return notecard;
            });
            return true;
         }); // end ShowPRogress
      } // end Import

   } // end MenuController
} // end UIController
