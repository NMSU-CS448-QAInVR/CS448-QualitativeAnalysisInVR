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

      public LoadMenuController LoadMenu;

      // locations
      [Header("Spawn Location")]
      [SerializeField]
      GameObject SpawnLocation;

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
         LoadMenu.PopulateSessionList(sessions, Load);
      } // end PopulateListView

      public void DeleteSession() {

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
         if (CardPrefabRenderer == null) {
            Debug.LogError("Cannot find renderer of card prefab");
            return;
         } // end if
         CardPrefabRenderer.material.SetColor("_Color", color.GetValue());

         GameObject newObj = GameObject.Instantiate(CardPrefab, SpawnLocation.transform.position, SpawnLocation.transform.rotation);
         saveLoadSys.Add(newObj);
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
      private async void ShowProgress(string prompt, string done_prompt_true, string done_prompt_false, Func<bool> operation) {
         
         // Set the UI components
         ProgressMenu.SetPrompt(prompt);
         Show(ProgressMenu);
         
         // Run the task
         Debug.Log("Running");
         ProgressMenu.ShowOnProgress(operation);
         bool result = operation();
         Debug.Log("Hello");

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
         Debug.Log("In Prompt");

         PromptMenu.SetPrompt(prompt);
         PromptMenu.SetButtonActions(delegate {action();}, delegate {PromptMenu.Hide();});

         Debug.Log("Show Prompt");
         Show(PromptMenu);
      } // end ShowPrompt

      public void Save() {
         saveLoadSys.SaveOnQuest(saveLoadSys.GetCurrentPath());
      } // end Save

      public void SaveAs(GameObject obj) {
         TextMeshPro input = obj.GetComponent<TextMeshPro>();
         if (input == null) 
            return;
         SaveAs(input.text + ".dat");
      } // end SaveAs
      private void SaveAs(string path) {
         saveLoadSys.SaveOnQuest(path, true);
      } // end SaveAs

      public void Load(string path) {
            ShowPrompt("Do you want to load: " + path, delegate {
               ShowProgress("Loading...", "File is loaded successfully", "The file is empty", () => {
                  bool result = PLoadSession(path);
                  return result;
               }); // end ShowProgress
            }); // end ShowPrompt
      } // end Load

      private bool PLoadSession(string path) {
         Debug.Log("I'm here");
         Delete();
         List<SaveFormat> items = saveLoadSys.LoadFromQuest(path);
         if (items == null) {
            Debug.LogError("The loaded items is empty");
            return false;
         } // end if

         foreach (SaveFormat item in items) {
            GameObject obj = null;
            if (item.getType() == FormatType.NOTECARD) {
               obj = (GameObject) GameObject.Instantiate(CardPrefab, new Vector3(0, 1, 0), Quaternion.Euler(0, 0, 0));
            } else {

            } // end else
            
            if (obj != null) {
               item.LoadObjectInto(obj);
               saveLoadSys.Add(obj);
            } // end if
         } // end foreach

         return true;
      } // end PLoadSession
      
      public void Delete() {
         foreach (GameObject obj in saveLoadSys.objects) {
               GameObject.Destroy(obj);
         } // end for each
         saveLoadSys.Clear();
      } // end Delete
      
      private void Hide(BaseSubMenuController menu) {
         menu.Hide();
      } // end Hide

      private void Show(BaseSubMenuController menu) {
         menu.Show();
      } // end Show
   } // end MenuController

} // end UIController
