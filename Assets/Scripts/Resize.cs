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

    public LineRenderer line;

    bool cardHeld = false;
    //bool cardSizeActive = false;
    bool placed = false;
    
     public GameObject card;
     public Rigidbody rb;

     
    public void Awake() {
    } // end Awake
 
    
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

    public void snap(){
         
    }

    public void triggered(){
        Debug.Log("trigger pressed");
        placed = true;   
        
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
            

        

        if(card != null && cardHeld == true){
                //Debug.Log("here");
             //Physics.Raycast(endpos, endpos - startPos, out hit, 40, LayerMask.GetMask("Everything"));
             //Debug.DrawRay(endpos, lineLength, Color.yellow);
            Vector3 endpos = line.GetPosition(line.positionCount - 1);
            Vector3 startPos = line.GetPosition(0);
            Vector3 lineLength = new Vector3(30, 30 ,30);
            Vector3 direction = Vector3.Normalize(endpos - startPos);
            Vector3 collisionPadding = direction * 0.3f;

            
            RaycastHit hit;
          if (Physics.Raycast(endpos + collisionPadding , direction, out hit, 30, LayerMask.GetMask("Board"))) {
                Debug.Log("here in raycast");
                DrawLine(endpos + collisionPadding, hit.point, Color.blue);
                 //Debug.DrawRay(endpos, endpos-startPos, Color.yellow);
                 //Debug.Log("ray collided!");
                 
                 if(placed == true){
                 card.transform.position = hit.point;
                 card.transform.localEulerAngles = new Vector3(hit.transform.localEulerAngles.x, hit.transform.localEulerAngles.y + 90, hit.transform.localEulerAngles.z);
    
                 //rb.constraints = RigidbodyConstraints.FreezePosition;
                 
                 }
                
           
            }

            //set rotation to be flat against surface
                
                // new Vector3(0,90,0);
            //lock x position
            
            
            //Debug.DrawRay (gameObject.transform.position, transform.right, Color.red, 5);
            //resize with buttons code
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

    else{}

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

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.01f)
         {
             GameObject myLine = new GameObject();
             myLine.transform.position = start;
             myLine.AddComponent<LineRenderer>();
             LineRenderer lr = myLine.GetComponent<LineRenderer>();
             lr.positionCount = 2;
             //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
             lr.startColor = color;
             lr.endColor = color;
             lr.startWidth = 0.01f;
             lr.endWidth = 0.01f;
             lr.SetPosition(0, start);
             lr.SetPosition(1, end);
             GameObject.Destroy(myLine, duration);
         }

}

