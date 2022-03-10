using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

   private Renderer CardPrefabRenderer;
   private Renderer BoardPrefabRenderer;
   
   private GameObject current;

   private CategoryTypeEnum categoryType;

   void Awake() {
      current = this.gameObject;
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
      current = InitialMenu;
   } // end Awake

   public void GoToMenu(GameObject des) {
      Hide(current);
      Show(des);
      current = des;
   } // end SwapMenu

   public void SetCategoryType(CategoryType type) {
      categoryType = type.GetValue();
   } // end SetCategoryType

   public void CreateCard(ColorType color) {
      if (CardPrefabRenderer == null) {
         Debug.LogError("Cannot find renderer of card prefab");
         return;
      } // end if
      CardPrefabRenderer.material.SetColor("_Color", color.GetValue());

      Object.Instantiate(CardPrefab, new Vector3(0, 1, 0), Quaternion.Euler(0, 0, 0));
   } // end CreateCard

   public void CreateCategory(ColorType color) {
      if (BoardPrefabRenderer == null) {
         Debug.LogError("Cannot find renderer of board prefab");
         return;
      } // end if
      BoardPrefabRenderer.material.SetColor("_Color", color.GetValue());

      Object.Instantiate(BoardPrefab, new Vector3(0, 1, 0), Quaternion.Euler(0, 0, 0));
   } // end CreateCategory
   
   private void Hide(GameObject menu) {
      menu.SetActive(false);
   } // end Hide

   private void Show(GameObject menu) {
      menu.SetActive(true);
   } // end Show


} // end MenuController
