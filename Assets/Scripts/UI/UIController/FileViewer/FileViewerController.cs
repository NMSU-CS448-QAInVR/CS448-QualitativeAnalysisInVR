using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;   
using TMPro;

namespace UIController {   
   public class FileViewerController : MonoBehaviour
   {
      public GameObject content;

      public Text FolderName;

      public GameObject FileViewerEntryTemplate;

      public void ShowFilesAndDirectory(MyDirectory directory) {
         // files
         List<MyFile> files = directory.files;
         foreach (MyFile file in files) {
            CreateEntry(file.path);
         } // end foreach

         List<MyDirectory> dirs = directory.directories;
         foreach (MyDirectory dir in dirs) {
            CreateEntry(dir.path, true);
         } // end foreach

         UpdateFolderName(directory.name);
      } // end ShowFile

      private void CreateEntry(string path, bool isDir=false) {
         GameObject newEntry = GameObject.Instantiate(FileViewerEntryTemplate);
         newEntry.SetActive(true);

         FileViewerEntry fwe = newEntry.GetComponent<FileViewerEntry>();
         fwe.SetEntryName(FileManager.GetNameFromPath(path));
         if (isDir)
            fwe.ShowIcon();
         else
            fwe.HideIcon();

         newEntry.transform.SetParent(content.transform);
      } // end CreateEntry

      public void UpdateFolderName(string name) {
         // to be done
         FolderName.text = name;
      } // end LinkClicked
   }

} // end UIController
