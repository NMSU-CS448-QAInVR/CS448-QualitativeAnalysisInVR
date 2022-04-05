using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;   
using TMPro;

namespace UIController {
   public class ContextualMenuController : MonoBehaviour
   {
      private FormatType targetType;
      private GameObject targetObject;


      void Awake() {
      } // end Awake

      public void SetNotecardFont(Slider slider) {
         if (targetType != FormatType.NOTECARD)
            return;

         NotecardTextEdit nte = targetObject.GetComponent<NotecardTextEdit>();
         if (nte == null)
            return;
         
         nte.SetTextFontSize(slider.value);
      } // end SetNotecardFont

      public void SetTargetObject(GameObject obj, FormatType type) {
         if (obj == null)
            return;
         
         targetObject = obj;
         targetType = type;
      } // end GetHostObject

   } // end MenuController

} // end UIController
