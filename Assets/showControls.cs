using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class showControls : MonoBehaviour
{

    public GameObject Controls;
    //private SaveLoadSystem sys;
    //private GameObject obj;
    bool isOpen = false;

    public void Clicked() {
        if(isOpen == false){
            Debug.Log("HERE");

            Controls.SetActive(true);

            //obj = Object.Instantiate(Controls, new Vector3(6.073f, 1.041f, 2.688f), Quaternion.Euler(0, 90, 0));
            ///sys.Add((GameObject) obj);
            isOpen = true;}
        else{
            Controls.SetActive(false);
            isOpen = false;
        }
    }
  
}
