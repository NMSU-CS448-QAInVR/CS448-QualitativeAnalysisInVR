using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

//CONTROLS CARD RESIZING WITH A AND B AS WELL AS BOARD SNAPPING FUNCTIONALITY
//SECONDARY RAYCAST DRAWN IN THIS SCRIPT
//05/04/2022

//Add this script to objects that need to be placed on surfaces from a distance.
//Line 91 specifies layer mask for objects that can be detected. 

public class Resize : MonoBehaviour
{
    
    public InputActionReference sizePrimary = null;  //size button connection 1
    public InputActionReference sizeSecondary = null; //size button connection 2

    private float HandDistanceVert;   
    private float HandDistanceHoriz;

    public LineRenderer line;   //connection to raycast for positioning

    public Material lineMat;    //connection for line material

    bool cardHeld = false;     //is card held
    //bool cardSizeActive = false;
    bool placed = false;       //is card placed 
    
     public GameObject card;   //connection to card object
     public Rigidbody rb;      //connection to rigidbody of card object

     
    public void Awake() {
    } // end Awake
 
    
    //called when trigger is pressed (set in xr interactor component)
    public void Clicked(){
       
        //Debug.Log("clicked");
        cardHeld = true;
        placed = false;  //so card can be moved again
        rb.constraints = RigidbodyConstraints.None;

        
    }

    //called when trigger is released
    public void LetGo(){
        
        //Debug.Log("let go");
        cardHeld = false;
    }

  
    public void triggered(){
       // Debug.Log("trigger pressed");
        placed = true;   
        
       
    }

    public void unTriggered(){   
    }

    void Start()
    {
        Debug.Log("in resize");
        
    }

    // Update is called once per frame
    void Update()
    {
            
        //only do if card is currently held
        if(card != null && cardHeld == true){
                
            Vector3 endpos = line.GetPosition(line.positionCount - 1);   //end pos of builtin ray interactor
            Vector3 startPos = line.GetPosition(0);                      //start position of builtin ray interactor
            
            Vector3 direction = Vector3.Normalize(endpos - startPos);    //to get direction of ray interactor and length
            Vector3 collisionPadding = direction * 0.3f;                 //add space bewteen held object and ray interactor 

            
            RaycastHit hit;  
            
            //create ray cast and check for collisions with objects in the Board layer mask 
          if (Physics.Raycast(endpos + collisionPadding , direction, out hit, 30, LayerMask.GetMask("Board"))) {     //returns hit object
                //Debug.Log("here in raycast");
               
               //draw line for user if collision is detected
                DrawLine(endpos + collisionPadding, hit.point, Color.blue);
                 
                 
                 if(placed == true){  //if trigger is pressed. 
                 card.transform.position = hit.point;   //move card to loaction of collision between raycast and board 
                 //switch card rotation to the same as hit object rotation
                 card.transform.localEulerAngles = new Vector3(hit.transform.localEulerAngles.x, hit.transform.localEulerAngles.y + 90, hit.transform.localEulerAngles.z);
                 //freeze card position while placed is true
                 rb.constraints = RigidbodyConstraints.FreezePosition;
                 
                 }//of if placed = true
                
           
            }
 
            
            //Debug.DrawRay (gameObject.transform.position, transform.right, Color.red, 5);

            //resize with buttons code
            Vector3 objectScale = transform.localScale;
            float value = sizePrimary.action.ReadValue<float>();
            float value2 = sizeSecondary.action.ReadValue<float>();
            size(value, value2, objectScale);  

   }//of cardHeld



    }// of update

    //resize function called
    void size(float value, float value2, Vector3 objectScale){

        if(value > 0){  //if float value returned from button press is not zero
           // Debug.Log("Primary right pressed");
            //Debug.Log(value); 
            transform.localScale = new Vector3(objectScale.x/1.009f, objectScale.y/1.009f, objectScale.z);
        }

        if(value2 > 0){ //if float value returned from button press is not zero
            //Debug.Log("secondary right pressed");
           // Debug.Log(value); 
            transform.localScale = new Vector3(objectScale.x*1.009f, objectScale.y*1.009f, objectScale.z);
        }
        
    }// of size

    //line specificatons 
    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.01f)
         {
             GameObject myLine = new GameObject();
             myLine.transform.position = start;
             myLine.AddComponent<LineRenderer>();
             LineRenderer lr = myLine.GetComponent<LineRenderer>();
             lr.positionCount = 2;
             lr.material = lineMat;
             lr.startColor = color;
             lr.endColor = color;
             lr.startWidth = 0.01f;
             lr.endWidth = 0.01f;
             lr.SetPosition(0, start);
             lr.SetPosition(1, end);
             GameObject.Destroy(myLine, duration);
         }

}

