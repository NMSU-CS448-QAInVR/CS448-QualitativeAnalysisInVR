using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

/*
    An object to assign to the session button in session list.
*/
public class ListViewButton : MonoBehaviour
{   
    /*
        Update the text on the button
        Input: 
        + text: a string to update the text with.
    */
    public void UpdateText(string text) {
        Text textObj = GetComponentInChildren<Text>();
        if (text == null)
            return;
        
        textObj.text = text;
    } // end UpdateTExt

    /*
        Add another action to do on click of the button
        Input: 
        + func: A UnityAction to set the on click action with.
    */
    public void SetOnClick(UnityAction func) {
        Button myButton = this.GetComponent<Button>();
        if (myButton == null) 
            return;
        
        myButton.onClick.AddListener(func);
    }
}
