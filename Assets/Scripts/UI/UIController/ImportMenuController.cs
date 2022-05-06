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
      The controller for import menu. 
   */
   public class ImportMenuController : BaseSubMenuController
   {
      // The action to execute when the Import button of ImportMenu is clicked.
      public UnityEvent<string, bool> OnImportAction;

      // the file browser of the Import Menu
      [SerializeField]
      private FileViewerController FileViewerController;

      // The directory we're currently at.
      private MyDirectory CurrentDirectory;
      // The selected file.
      private MyFile SelectedFile;
      // A boolean value to check if we will parse notecard or not. This is currently not used.
      private bool WillParseToNotecard;

      /*
         Check that we will parse the notecard
      */
      public void UpdateParseToNotecard(bool value) {
         WillParseToNotecard = value;
      } // end UpdateParseToNotecard

      /*
         The action to execute when the Import button is pressed.
      */
      public void Import() {
         OnImportAction.Invoke(FileViewerController.GetSelectedFilePath(), true);
      } // end Import

      /*
         When this submenu is set active again, refresh the file list.
      */
      public void OnEnable() {
         RefreshFilesList();
      } // end OnEnable

      /*
         Refresh the file list.
      */
      public void RefreshFilesList() {
         FileViewerController.Refresh();
      } // end RefreshList
   }

} // end UIController
