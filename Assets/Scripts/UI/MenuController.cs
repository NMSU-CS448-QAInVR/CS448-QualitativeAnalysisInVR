using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
   public GameObject MainMenu;
   public GameObject CreateMenu;
   public GameObject CreateCardMenu;
   public GameObject CategoryColorMenu;
   public GameObject CategoryTypeMenu;
   public GameObject CardPrefab;
   public GameObject BoardPrefab;
   public GameObject InitialMenu;
   public GameObject ListViewButtonTemplate;
   public GameObject ListViewContentObject;
   [SerializeField]
   GameObject SpawnLocation;

   [Range(1, 10)]
   public int max_sessions = 1;


   private Renderer CardPrefabRenderer;
   private Renderer BoardPrefabRenderer;
   
   private List<UISession> sessions;

   private int currentSessionID;
   
   private UISession currentSession;
   private SaveLoadSystem saveLoadSys;
   

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
    } // end Load

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
