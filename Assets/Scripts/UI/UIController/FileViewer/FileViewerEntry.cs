using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;   
using TMPro;

public class FileViewerEntry: MonoBehaviour {
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text entryName;
    
    private MyFileOrDirectory entry;

    public UnityEvent<string> OnDirectoryClickedAction;

    public UnityEvent<MyFile> OnFileClickedAction;

    public void ShowIcon() {
        icon.gameObject.SetActive(true);
    } // end ShowIcon

    public void HideIcon() {
        icon.gameObject.SetActive(false);
    } // end HideIcon

    public void SetUpFile(MyFile dir) {
        entry = dir;
        SetEntryName(entry.name);
        HideIcon();
    } // end SetUp

    public void SetUpDir(MyDirectory dir) {
        entry = dir;
        SetEntryName(entry.name);
        ShowIcon();
    } // end SetUpDir

    public string GetPath() {
        return entry.path;
    } // end GetPath

    public void Clicked() {
        if (entry.GetIsDirectory())
            OnDirectoryClickedAction.Invoke(entry.path);
        else {
            OnFileClickedAction.Invoke((MyFile) entry);
        }
    } // end Clicked


    public void SetEntryName(string text) {
        entryName.text = text;
    } // end 
}