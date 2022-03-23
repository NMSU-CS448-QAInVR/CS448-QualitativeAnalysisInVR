using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Resize : MonoBehaviour
{
    
    public InputActionReference sizePrimary = null;
    public InputActionReference sizeSecondary = null;

    private float HandDistanceVert;
    private float HandDistanceHoriz;

    bool cardHeld = false;
    bool cardSizeActive = false;

    //[SerializeField]
     public GameObject card;

    // Gets the local scale of a game object
    
    public void Clicked(){
       
        Debug.Log("clicked");
        cardHeld = true;
    }

    public void LetGo(){
        
        Debug.Log("let go");
        cardHeld = false;
    }

    public void triggered(){
        Debug.Log("trigger pressed");
        //Debug.Log(GetPosition.LX);
        //Debug.Log(GetPositionR.RX);
        cardSizeActive = true;  
    }

    public void unTriggered(){
        cardSizeActive = false;
    }

    void Start()
    {
        Debug.Log("in resize");
        
        
    }

    // Update is called once per frame
    void Update()
    {


       if(card != null && cardHeld == true){
        
       Vector3 objectScale = transform.localScale;
       float value = sizePrimary.action.ReadValue<float>();
       float value2 = sizeSecondary.action.ReadValue<float>();
       size(value, value2, objectScale);  

       if(cardSizeActive == true){
           HandDistanceHoriz = System.Math.Abs(GetPosition.LX - GetPositionR.RX);
           HandDistanceVert = System.Math.Abs(GetPosition.LY - GetPositionR.RY);
           transform.localScale = new Vector3(HandDistanceHoriz, HandDistanceVert, objectScale.z);}

       }     
    }

    void size(float value, float value2, Vector3 objectScale){

        if(value > 0){
           // Debug.Log("Primary right pressed");
            //Debug.Log(value); 
            transform.localScale = new Vector3(objectScale.x/1.009f, objectScale.y/1.009f, objectScale.z);
        }

        if(value2 > 0){
            //Debug.Log("secondary right pressed");
           // Debug.Log(value); 
            transform.localScale = new Vector3(objectScale.x*1.009f, objectScale.y*1.009f, objectScale.z);
        }
        
    }// of size

  

}
