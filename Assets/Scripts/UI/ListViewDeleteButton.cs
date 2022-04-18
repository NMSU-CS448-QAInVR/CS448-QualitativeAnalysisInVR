using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class ListViewDeleteButton : MonoBehaviour
{
    public void SetOnClick(UnityAction func) {
        Button myButton = this.GetComponent<Button>();
        if (myButton == null) 
            return;
        
        myButton.onClick.AddListener(func);
    }
}
