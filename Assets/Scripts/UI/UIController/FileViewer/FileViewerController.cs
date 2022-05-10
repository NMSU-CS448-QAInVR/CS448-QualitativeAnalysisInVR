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
   /*
      The controller for the file viewing. 
   */
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

      /*
         Refresh the list of files
      */
      public void Refresh() {
         GoToFolder(CurrentDir.path);
      } // end Refresh

      /*
         Populate the file list with the files and subdirectories. 
         Input:
         + directory: the directory to show the files and subdirectories of. 
      */
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

      /*
         Clear the current list of files and subdirectories. Destroy all entry objects. 
      */
      private void ClearCurrentList() {
         int count = entries.Count;
         for (int i = 0; i < count; ++i) {
            GameObject.Destroy(entries.First.Value);
            entries.RemoveFirst();
         } // end for i
      } // end ClearCurrentList

      /*
         A function for the entry to go to a folder
      */
      public void EntryGoToFolder(string path) {
         GoToFolder(path, true);
      } // end EntryGoToFolder

      /*
         Update the file list to a new folder.
         Input: 
         + path: the path of the new folder
         + changeHistory: Whether or not to overwrite the file viewing history. 
      */
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

      /*
         Create a new file/dir entry in the list.

         Input:
         + path: the path of the entry
         + isDir: is this path a directory or a file.
         + dir: The directory information object if the path is a directory. 
         + fl: The file information object if the path is a file. 
      */
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

      /*
         Update the Text showing the current folder. 
         Input:
         + name: the name to update with
      */
      public void UpdateFolderName(string name) {
         FolderName.text = name;
      } // end LinkClicked

      /*
         Go to the previous directory in the history.
      */
      public void GoToPrevDir() {
         forward.Push(CurrentDir);
         MyDirectory target = prev.Pop();
         if (target != null)
            GoToFolder(target.path);
      } // end GoToParentDir

      /*
         Go to the forward directory in the history.
      */
      public void GoToForwardDir() {
         prev.Push(CurrentDir);
         MyDirectory target = forward.Pop();
         if (target != null)
            GoToFolder(target.path);
      } // end GoToParentDir

      /*
         Set the selected file.
      */
      public void SetSelectedFile(MyFile file) {
         selectedFile = file;
      } // end SetSelectedFile

      /*
         Get the file that is selected.
         Output: A string of the path of the file. An empty string if no file has been selected.
      */
      public string GetSelectedFilePath() {
         if (selectedFile != null)
            return selectedFile.path;

         return "";
      } // end GetSelectedFilePath
   }

} // end UIController
