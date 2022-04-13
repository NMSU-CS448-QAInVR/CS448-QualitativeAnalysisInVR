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
    private Text name;

    public void ShowIcon() {
        icon.gameObject.SetActive(true);
    } // end ShowIcon

    public void HideIcon() {
        icon.gameObject.SetActive(false);
    } // end HideIcon

    public void SetEntryName(string text) {
        name.text = text;
    } // end 
}