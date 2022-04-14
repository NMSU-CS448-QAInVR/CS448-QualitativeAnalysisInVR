using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;   
using TMPro;


namespace UIController {
   public class ImportMenuController : BaseSubMenuController
   {
      public UnityEvent<bool, string> OnImportAction;

      private MyDirectory CurrentDirectory;
      private MyFile SelectedFile;
      private bool WillParseToNotecard;

      public void UpdateParseToNotecard(bool value) {
         WillParseToNotecard = value;
      } // end UpdateParseToNotecard

      public void Import() {
         // to be done
      } // end Import
   }

} // end UIController
