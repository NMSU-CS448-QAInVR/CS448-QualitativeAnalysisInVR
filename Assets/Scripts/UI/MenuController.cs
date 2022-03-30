using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;   
using TMPro;

public class MenuController : MonoBehaviour
{
   // menus
   [Header("Menus Canvas")]
   public GameObject MainMenu;
   public GameObject CreateMenu;
   public GameObject CreateCardMenu;
   public GameObject CategoryColorMenu;
   public GameObject CategoryTypeMenu;
   public GameObject InitialMenu;
   public GameObject PromptMenu;
   public GameObject ProgressMenu;

   // List
   [Header("List View")]
   public GameObject ListViewButtonTemplate;
   public GameObject ListViewContentObject;

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
   } // end Awake

   public void GoToMenu(GameObject des) {
      if (des == null) {
         Debug.LogError("The next menu is null");
         return;
      } // end if
      Hide(currentSession.GetCurrent());
      Show(currentSession.MoveToNewMenu(des));
   } // end SwapMenu

   public void GoPrevMenu() {
      GameObject myCurrent = currentSession.GetCurrent();
      GameObject des = currentSession.MoveToPrevMenu();
      if (des == null)
         return;

      Hide(myCurrent);
      Show(des);
   } // end GoPrevMenu

   public void PopulateSessionsListView() {
         List<string> sessions = saveLoadSys.GetSessionsList();
         Transform[] children = ListViewContentObject.GetComponentsInChildren<Transform>();
         // remove existing buttons
         for (int i = 0; i < children.Length; ++i) {
            Transform child = children[i];
            if (child.parent.gameObject != ListViewContentObject || child.gameObject == ListViewButtonTemplate) {
               continue;
            } // end if
               
            Object.Destroy(child.gameObject);
         } // end foreach

         // add new buttons
         foreach (string session in sessions) {
            GameObject button = Object.Instantiate(ListViewButtonTemplate);
            button.transform.SetParent(ListViewButtonTemplate.transform.parent, false);
            ListViewButton lvb = button.GetComponentInChildren<ListViewButton>();
            lvb.UpdateText(session.Substring(0, session.Length - 4));
            lvb.SetOnClick(delegate {Load(session);});
            button.SetActive(true);
         } // end foreach
         ListViewButtonTemplate.SetActive(false);
    } // end PopulateListView

   public void DeleteSession() {

   } // end DeleteSession

   //public void DeleteSession(GameObject, )

   public void GoForwdMenu() {
      GameObject myCurrent = currentSession.GetCurrent();
      GameObject des = currentSession.MoveToForwardMenu();
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

      GameObject newObj = Object.Instantiate(CardPrefab, SpawnLocation.transform.position, SpawnLocation.transform.rotation);
      saveLoadSys.Add(newObj);
   } // end CreateCard

   public void CreateCategory(ColorType color) {
      if (BoardPrefabRenderer == null) {
         Debug.LogError("Cannot find renderer of board prefab");
         return;
      } // end if
      BoardPrefabRenderer.material.SetColor("_Color", color.GetValue());

      Object.Instantiate(BoardPrefab, SpawnLocation.transform.position, SpawnLocation.transform.rotation);
   } // end CreateCategory

   /*
      return an array of 2 elements that contain the buttons Yes/No of the prompt menu. 
      array[0] is the yes button
      array[1] is the no button
      precodnition: menu is not null
   */
   private Button[] PromptExtractButtons(GameObject menu) {
      Button[] result = new Button[2];

      // extract the yes button
      Button[] temp = menu.GetComponentsInChildren<Button>();
      if (temp.Length > 2 || temp.Length < 2) {
         return result;
      } // end if
      if (temp[0].name.ToLower() == "no") {
         Button t = temp[0];
         temp[0] = temp[1];
         temp[1] = t;
      } // end if

      result = temp;

      return result;
   } // end PromptExtractbutton

   /*
      return the prompt object of the menu. 
      precodnition: menu is not null
   */
   private TextMeshProUGUI GetPromptField(GameObject menu) {
      return menu.GetComponentInChildren<TextMeshProUGUI>();
   } // end GetPromptField

   public void ShowPrompt(string prompt, UnityAction action) {
      Debug.Log("In Prompt");
      TextMeshProUGUI promptField = GetPromptField(PromptMenu);
      if (promptField == null)
         return;

      promptField.SetText(prompt);

      Button[] buttons = PromptExtractButtons(PromptMenu);
      // yes button
      if (buttons[0] != null)
         buttons[0].onClick.AddListener(delegate {action();});
      // no button
      if (buttons[1] != null)
         buttons[1].onClick.AddListener(delegate {PromptMenu.SetActive(false);});

      Debug.Log("Show Prompt");
      PromptMenu.SetActive(true);
   } // end ShowPrompt

   public bool ShowPrompt(string prompt) {
       
      TextMeshProUGUI promptField = GetPromptField(PromptMenu);
      if (promptField == null)
         return false;
      
      Button[] buttons = PromptExtractButtons(PromptMenu);
      // yes button
      if (buttons[0] != null)
         buttons[0].onClick.AddListener(delegate {;});
      // no button
      if (buttons[1] != null)
         buttons[1].onClick.AddListener(delegate {PromptMenu.SetActive(false);});

      PromptMenu.SetActive(true);
      return false;
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
            PLoadSession(path);
         });
    } // end Load

   private void PLoadSession(string path) {
      Delete();
        List<SaveFormat> items = saveLoadSys.LoadFromQuest(path);
        if (items == null) {
            Debug.LogError("The loaded items is empty");
            return;
        } // end if

        foreach (SaveFormat item in items) {
            GameObject obj = null;
            if (item.getType() == FormatType.NOTECARD) {
               obj = (GameObject) Object.Instantiate(CardPrefab, new Vector3(0, 1, 0), Quaternion.Euler(0, 0, 0));
            } else {

            } // end else
          
            if (obj != null) {
               item.LoadObjectInto(obj);
               saveLoadSys.Add(obj);
            } // end if
        } // end foreach
   } // end PLoadSession

   

    public void Delete() {
        foreach (GameObject obj in saveLoadSys.objects) {
            Object.Destroy(obj);
        } // end for each
        saveLoadSys.Clear();
    } // end Delete
   
   private void Hide(GameObject menu) {
      menu.SetActive(false);
   } // end Hide

   private void Show(GameObject menu) {
      menu.SetActive(true);
   } // end Show


} // end MenuController
