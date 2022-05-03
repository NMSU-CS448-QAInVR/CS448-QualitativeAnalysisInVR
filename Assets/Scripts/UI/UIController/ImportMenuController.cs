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
      public UnityEvent<string, bool> OnImportAction;

      [SerializeField]
      private FileViewerController FileViewerController;

      private MyDirectory CurrentDirectory;
      private MyFile SelectedFile;
      private bool WillParseToNotecard;

      public void UpdateParseToNotecard(bool value) {
         WillParseToNotecard = value;
      } // end UpdateParseToNotecard

      public void Import() {
         OnImportAction.Invoke(FileViewerController.GetSelectedFilePath(), true);
      } // end Import

      public void OnEnable() {
         RefreshFilesList();
      } // end OnEnable

      public void RefreshFilesList() {
         FileViewerController.Refresh();
      } // end RefreshList
   }

} // end UIController
