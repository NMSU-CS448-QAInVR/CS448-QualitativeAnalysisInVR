using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class changeSky : MonoBehaviour
{

    public Material box1;
    public Material box2;
    public Material box3;

    private int currentBox = 1;

    // Start is called before the first frame update
  public void Switch(){
      
      if (currentBox == 1){ 
          currentBox = 2;
          RenderSettings.skybox = box2;}
      else if(currentBox == 2){
          currentBox = 3;
          RenderSettings.skybox = box3;
      }
      else{RenderSettings.skybox = box1;
            currentBox = 1;
      }
  }

}
