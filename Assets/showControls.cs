using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//controls info page controller

public class showControls : MonoBehaviour
{

    public GameObject Controls;
    //private SaveLoadSystem sys;
    //private GameObject obj;
    bool isOpen = false; // is controls open initially false

    public void Clicked() {
        if(isOpen == false){
            //Debug.Log("HERE");

            Controls.SetActive(true); // if clickes set to active

            
            isOpen = true;}
        else{
            Controls.SetActive(false);
            isOpen = false;
        }
    }
  
}
