using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;   
using TMPro;
using UnityEngine.Android;

namespace UIController {   
   public class FileViewerController : MonoBehaviour
   {
      public GameObject content;

      public Text FolderName;

      public GameObject FileViewerEntryTemplate;

      private string path = "/";
      private MyDirectory CurrentDir = null;

      private bool isAndroid = false;

      private LinkedList<GameObject> entries = null;

      private Stack<MyDirectory> prev = null;
      private Stack<MyDirectory> forward = null;

      private MyFile selectedFile = null;
      
      public void Awake() {
         #if UNITY_ANDROID && !UNITY_EDITOR
         isAndroid = true;
         path = FileManager.GetDataPath();
         #else
         path = "\\Users\\Tom\\Documents\\VRCSVS";
         #endif
         


         entries = new LinkedList<GameObject>();
         prev = new Stack<MyDirectory>();
         forward = new Stack<MyDirectory>();
         GoToFolder(path);
      } // end Awake

      private void ShowFilesAndDirectory(MyDirectory directory) {
         // files
         List<MyFile> files = directory.files;
         foreach (MyFile file in files) {
            CreateEntry(file.path, fl:file);
         } // end foreach

         List<MyDirectory> dirs = directory.directories;
         Debug.Log(dirs == null);
         foreach (MyDirectory dir in dirs) {
            CreateEntry(dir.path, true, dir);
         } // end foreach

         UpdateFolderName(directory.name);
      } // end ShowFile

      private void ClearCurrentList() {
         int count = entries.Count;
         for (int i = 0; i < count; ++i) {
            GameObject.Destroy(entries.First.Value);
            entries.RemoveFirst();
         } // end for i
      } // end ClearCurrentList

      public void EntryGoToFolder(string path) {
         GoToFolder(path, true);
      } // end EntryGoToFolder

      public void GoToFolder(string path, bool ChangeHistory=false) {
         if (isAndroid) {
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
               Permission.RequestUserPermission(Permission.ExternalStorageRead);
         } // end if

         if (ChangeHistory) {
            forward.Clear();
            prev.Push(CurrentDir);
         } // end if
         
         ClearCurrentList();
         CurrentDir = new MyDirectory(path);
         ShowFilesAndDirectory(CurrentDir);
      } // end GoToFolder

      private void CreateEntry(string path, bool isDir = false, MyDirectory dir = null, MyFile fl = null) {
         Debug.Log("Create Entry");
         GameObject newEntry = GameObject.Instantiate(FileViewerEntryTemplate);
         newEntry.SetActive(true);
         entries.AddFirst(newEntry);
         newEntry.transform.SetParent(content.transform, false);

         FileViewerEntry fwe = newEntry.GetComponent<FileViewerEntry>();
         if (isDir)
            fwe.SetUpDir(dir);
         else
            fwe.SetUpFile(fl);         
      } // end CreateEntry

      public void UpdateFolderName(string name) {
         FolderName.text = name;
      } // end LinkClicked

      public void GoToPrevDir() {
         forward.Push(CurrentDir);
         MyDirectory target = prev.Pop();
         if (target != null)
            GoToFolder(target.path);
      } // end GoToParentDir

      public void GoToForwardDir() {
         prev.Push(CurrentDir);
         MyDirectory target = forward.Pop();
         if (target != null)
            GoToFolder(target.path);
      } // end GoToParentDir

      public void SetSelectedFile(MyFile file) {
         selectedFile = file;
      } // end SetSelectedFile

      public string GetSelectedFilePath() {
         if (selectedFile != null)
            return selectedFile.path;

         return "";
      } // end GetSelectedFilePath
   }

} // end UIController
