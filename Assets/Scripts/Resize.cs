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
    bool placed = false;
    

    //[SerializeField]
     public GameObject card;
     public Rigidbody rb;
    // Gets the local scale of a game object
    
    public void Clicked(){
       
        Debug.Log("clicked");
        cardHeld = true;
        placed = false;
        rb.constraints = RigidbodyConstraints.None;
    }

    public void LetGo(){
        
        Debug.Log("let go");
        cardHeld = false;
    }

    public void triggered(){
        Debug.Log("trigger pressed");


        // check if card is currently passed placing boundry larger number is closer to board smaller is farther away
        if( card.transform.position.x > .45){  
            placed = true;   //boards will likely each need there own value for this.
        }

        //cardSizeActive = true;  
    }

    public void unTriggered(){
        //cardSizeActive = false;
    }

    void Start()
    {
        Debug.Log("in resize");
        
        
    }

    // Update is called once per frame
    void Update()
    {


        if(placed == true){
            //set z position for pos board | 6.75 = just off surface. may need to be updated or refrenced from board if object loation is moved.
            card.transform.position = new Vector3(6.75f, card.transform.position.y, card.transform.position.z);
            //set rotation to be flat against surface
            card.transform.localEulerAngles = new Vector3(0,90,0);
            //lock x position
            rb.constraints = RigidbodyConstraints.FreezePositionX;
            //lock rotation

        }

       if(card != null && cardHeld == true){
        
       Vector3 objectScale = transform.localScale;
       float value = sizePrimary.action.ReadValue<float>();
       float value2 = sizeSecondary.action.ReadValue<float>();
       size(value, value2, objectScale);  

    //    if(cardSizeActive == true){
    //        HandDistanceHoriz = System.Math.Abs(GetPosition.LX - GetPositionR.RX);
    //        HandDistanceVert = System.Math.Abs(GetPosition.LY - GetPositionR.RY);
    //        transform.localScale = new Vector3(HandDistanceHoriz, HandDistanceVert, objectScale.z);}

    //    }     
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
