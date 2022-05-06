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
      A controller for the contextual menu.
   */
   public class ContextualMenuController : MonoBehaviour
   {
      private FormatType targetType;
      private GameObject targetObject;

      // the contextual menu for notecards
      [SerializeField]
      GameObject NotecardMenu;


      void Awake() {
      } // end Awake

      /*
         Setting the font on the notecard. 
         This is not used.
      */
      public void SetNotecardFont(Slider slider) {
         if (targetType != FormatType.NOTECARD)
            return;

         NotecardTextEdit nte = targetObject.GetComponent<NotecardTextEdit>();
         if (nte == null)
            return;
         
         nte.SetTextFontSize(slider.value);
      } // end SetNotecardFont

      /*
         Set up the contextual menu for the notecard. Only Notecard is supported right now.
      */
      public void SetTargetObject(GameObject obj, FormatType type) {
         if (obj == null)
            return;
         
         targetObject = obj;
         targetType = type;

         if (type == FormatType.NOTECARD) {
            SetUpContextualMenuForNotecard();
         } else {

         }
      } // end GetHostObject

      /*
         Set up the contextual menu for notecard
      */
      private void SetUpContextualMenuForNotecard() {
         //Slider fontSizeSlider = NotecardMenu.GetComponentInChildren<Slider>();
         NotecardTextEdit nct = targetObject.GetComponent<NotecardTextEdit>();
         //fontSizeSlider.SetValueWithoutNotify(nct.GetTextFontSize());
      } // end Set

      /*
         Edit text for the notecard
      */
      public void EditText() {
         NotecardTextEdit nct = targetObject.GetComponent<NotecardTextEdit>();
         nct.InvokeEdit(true);
      } // EditText

      /*
         Edit Title for the notecard.
      */
      public void EditTitle() {
         NotecardTextEdit nct = targetObject.GetComponent<NotecardTextEdit>();
         nct.InvokeEdit(false);
      } // EditText
   } // end MenuController

} // end UIController
