using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class ListViewButton : MonoBehaviour
{
    public void UpdateText(string text) {
        Text textObj = GetComponentInChildren<Text>();
        if (text == null)
            return;
        
        textObj.text = text;
    } // end UpdateTExt

    public void SetOnClick(UnityAction func) {
        Button myButton = this.GetComponent<Button>();
        if (myButton == null) 
            return;
        
        myButton.onClick.AddListener(func);
    }
}
