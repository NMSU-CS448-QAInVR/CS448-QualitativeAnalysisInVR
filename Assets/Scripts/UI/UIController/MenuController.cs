using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;   
using TMPro;

namespace UIController {
   /*
      The controller of the main menu.
      There will be functions to show:
      + Prompt Menu: A yes/no menu for users to pick.
      + Progress Menu: A menu to show the progress and status of the task. 
   */
   public class MenuController : MonoBehaviour
   {
      // menus
      // the controller of each submenus.
      [Header("Menus Canvas")]
      public BaseSubMenuController MainMenu;
      public BaseSubMenuController CreateMenu;
      public BaseSubMenuController CreateCardMenu;
      public BaseSubMenuController CategoryColorMenu;
      public BaseSubMenuController CategoryTypeMenu;
      public BaseSubMenuController InitialMenu;
      public PromptMenuController PromptMenu;
      public ProgressMenuController ProgressMenu;

      // the controller of the contextual menu. This is not used.
      public ContextualMenuController ContextualMenu;

      public LoadMenuController LoadMenu;

      // the prefab for the import object. The import object is an object that will allow users to sequentially spawn cards from the imported text.
      public ImportCSVMod ImportObjectPrefab;

      // locations
      // the spawning locations of the notecards
      [Header("Spawn Location")]
      [SerializeField]
      GameObject SpawnLocation;
      
      [Header("Drawing Functions")]
      // the Draw3DController of the pen.
      [SerializeField]
      private DrawController3D Draw3DController;
      // the list of Savable of boards.
      public List<Savable> boards;
      // The location to spawn the pen.
      public GameObject PenSummonLocation;


      // session
      // variables to set up the sessions. The original idea is to have multiple sessions that user can switch around. However, there is only one session now.
      [Header("Session")]
      [Range(1, 10)]
      public int max_sessions = 1;
      private List<UISession> sessions;
      private int currentSessionID;
      private UISession currentSession;

      // prefab
      // the card prefab and the board prefab.
      // the original idea is for users to spawn as many boards as they want. but there is only one board now. 
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

      /*
         Initialize stuffs.

         Important Part:
         + call Initialize() of FileManger first to get persistentDataPath.
         + call Initialize() of saveLoadSys afterwards to make sure that the session folder is created.
      */
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

      /*
         Add all of the current boards to save load system.
      */
      void Start() {
         saveLoadSys.AddBoards(boards);
      } // end Start

      /*
         Go to a submenu.
      */
      public void GoToMenu(BaseSubMenuController des) {
         if (des == null) {
            Debug.LogError("The next menu is null");
            return;
         } // end if
         Hide(currentSession.GetCurrent());
         Show(currentSession.MoveToNewMenu(des));
      } // end SwapMenu

      /*
         Go to the previous menu.
      */
      public void GoPrevMenu() {
         BaseSubMenuController myCurrent = currentSession.GetCurrent();
         BaseSubMenuController des = currentSession.MoveToPrevMenu();
         if (des == null)
            return;

         Hide(myCurrent);
         Show(des);
      } // end GoPrevMenu

      /*
         Populate the session list view.
      */
      public void PopulateSessionsListView() {
         List<string> sessions = saveLoadSys.GetSessionsList();
         LoadMenu.PopulateSessionList(sessions, Load, DeleteSession);
      } // end PopulateListView

      /*
         Delete a session. 
         Show Prompt if they want to delete or not, and Show Progress if yes.
         Input:
         + sessionPath: the name of the session to remove. 
         + deleteAction: the action to do when we successfully delete the session.
      */
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

      /*
         Go to the forward menu in the history.
      */
      public void GoForwdMenu() {
         BaseSubMenuController myCurrent = currentSession.GetCurrent();
         BaseSubMenuController des = currentSession.MoveToForwardMenu();
         if (des == null)
            return;
         Hide(myCurrent);   
         Show(des);
      } // end GoForwdMenu
      
      /*
         Set the type of category. This is not used.
      */
      public void SetCategoryType(CategoryType type) {
         categoryType = type.GetValue();
      } // end SetCategoryType

      /*
         Create card with a specified color.
         Input:
         + color: the color to create a notecard with.
      */
      public void CreateCard(ColorType color) {
         CreateCardInternal(color.GetValue(), SpawnLocation.transform.position, SpawnLocation.transform.rotation);
      } // end CreateCard
      
      /*
         Summon the pen the pen spawn location.
      */
      public void SummonPen() {
         Draw3DController.gameObject.transform.position = PenSummonLocation.transform.position;
      } // end SummonPen

      /*
         A function to create a card with a color, position, and rotation, then save it to the SaveLoadSystem.
         Input:
         + color: the color of teh card.
         + position: the position of the card.
         + rotation: the rotation of the card.
      */
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

      /*
         Meant for creating a category, but this is not used.
      */
      public void CreateCategory(ColorType color) {
         if (BoardPrefabRenderer == null) {
            Debug.LogError("Cannot find renderer of board prefab");
            return;
         } // end if
         BoardPrefabRenderer.material.SetColor("_Color", color.GetValue());

         GameObject.Instantiate(BoardPrefab, SpawnLocation.transform.position, SpawnLocation.transform.rotation);
      } // end CreateCategory


      /*
         Show the progress menu to the user.
         Input: 
         + prompt: the prompt to show when the task is being executed.
         + done_prompt_true: the prompt to show when the task is done.
         + done_prompt_false: the prompt to show when the task fails. 
         + operation: the task to keep trakc of the progress.
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

      /*
         Show the prompt menu to the user.
         Input:
         + prompt: the prompt to ask questions to user.
         + action: the action to do when user chooses the answer "Yes".
      */
      public void ShowPrompt(string prompt, UnityAction action) {
         Show(PromptMenu);
         PromptMenu.SetPrompt(prompt);
         PromptMenu.SetButtonActions(delegate {action();}, delegate {PromptMenu.Hide();});
      } // end ShowPrompt

      /*
         Save the currently opened session.
      */
      public void Save() {
         ShowProgress("Saving session...", "Saving is successful", "Saving is not successful", async () => {
            try {
               saveLoadSys.AddDrawings(Draw3DController.GetLines());
               await saveLoadSys.SaveOnQuestAsync(saveLoadSys.GetCurrentPath());
               return true;
            } catch (Exception ex) {
               Debug.LogError(ex.Message);
               Debug.LogError(ex.StackTrace);
               return false;
            }
         });
      } // end Save

      /*
         Save the current session with a different name. 
         Input:
         + obj: The name input object.
      */
      public void SaveAs(GameObject obj) {
         TMP_InputField input = obj.GetComponent<TMP_InputField>();
         if (input == null) 
            return;
         SaveAs(input.text);
      } // end SaveAs
      
      /*
         The function that do the actual savings.
         Save the current session with a different name. 
         Input:
         + path: The name of the session.
      */
      private void SaveAs(string path) {
         ShowProgress("Saving session...", "Saving is successful", "Saving is not successful", async () => {
            try {
               saveLoadSys.AddDrawings(Draw3DController.GetLines());
               await saveLoadSys.SaveOnQuestAsync(path, true);
               return true;
            } catch (Exception ex) {
               Debug.LogError(ex.Message);
               Debug.LogError(ex.StackTrace);
               return false;
            }
         });
      } // end SaveAs
      
      /*
         The function load a session. It will show the Prompt menu and the Progress menu. 
         Input:
         + path: the name of the session.
      */
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

      /*
         The action to do the actual loading.
         This will delete all of the current objects first before loading in the new items.
         Input:
         + path: the name of the session.
      */
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
               addToSaveLoadSys = false;
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

      /*
         Delete all to-be-saved objects.
      */      
      public void Delete() {
         Debug.Log("Clear is called");
         saveLoadSys.AddDrawings(Draw3DController.GetLines());
         Draw3DController.ClearAllDrawingsList();
         saveLoadSys.Clear();
      } // end Delete
      
      /*
         Hide a submenu
      */
      private void Hide(BaseSubMenuController menu) {
         menu.Hide();
      } // end Hide

      /*
         Show a submenu.
      */
      private void Show(BaseSubMenuController menu) {
         menu.Show();
      } // end Show

      /*
         Import a CSV file. If the path does not lead to a CSV file, this function will do nothing. 
         Input: 
         + path: the path to the file to import.
         + willParse: whether or not to parse this object. This is currently not used.
      */
      public void Import(string path, bool willParse=false) {
         ShowProgress("Importing...", "Created the import file creator successfully", "Failed to import this file", async () => {
            if (!path.EndsWith(".csv")) {
               return false;
            } // end if

            // to be done
            //Debug.Log("my path is " + path);
            string myText = await FileManager.ReadStringFromAsync(path, false);
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
