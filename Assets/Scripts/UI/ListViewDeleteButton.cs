using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

/*
    An object assigned to the delete button of the session list.
*/
public class ListViewDeleteButton : MonoBehaviour
{   
    /*
        Add an action to execute on click.
    */
    public void SetOnClick(UnityAction func) {
        Button myButton = this.GetComponent<Button>();
        if (myButton == null) 
            return;
        
        myButton.onClick.AddListener(func);
    }
}
