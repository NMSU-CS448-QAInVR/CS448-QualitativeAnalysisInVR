using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;   
using TMPro;

/*
    A script assigned to a file viewing entry to control its behaviour.
*/
public class FileViewerEntry: MonoBehaviour {
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text entryName;
    
    private MyFileOrDirectory entry;
    // the action do when a directory is clicked. 

    public UnityEvent<string> OnDirectoryClickedAction;
    // the action to do when a file is clicked.

    public UnityEvent<MyFile> OnFileClickedAction;

    /*
        Show the directory icon
    */
    public void ShowIcon() {
        icon.gameObject.SetActive(true);
    } // end ShowIcon

    /*
        Hide the directory icon
    */
    public void HideIcon() {
        icon.gameObject.SetActive(false);
    } // end HideIcon
    
    /*
        Set up this entry as a file. 
        Input:
        + dir: the file information to set up. 
    */
    public void SetUpFile(MyFile dir) {
        entry = dir;
        SetEntryName(entry.name);
        HideIcon();
    } // end SetUp

    /*
        Set up this entry as a directory.
        Input:
        + dir: the directory information to set up. 
    */
    public void SetUpDir(MyDirectory dir) {
        entry = dir;
        SetEntryName(entry.name);
        ShowIcon();
    } // end SetUpDir

    /*
        Get the path of this entry
        Output: the path of this entry
    */
    public string GetPath() {
        return entry.path;
    } // end GetPath

    /*
        The action to do when this entry is clicked. 
    */
    public void Clicked() {
        if (entry.GetIsDirectory())
            OnDirectoryClickedAction.Invoke(entry.path);
        else {
            OnFileClickedAction.Invoke((MyFile) entry);
        }
    } // end Clicked

    /*
        Set the entry name.
        Input:
        + text: the name to set the entry name.
    */
    public void SetEntryName(string text) {
        entryName.text = text;
    } // end 
}