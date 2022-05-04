using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Determines text display based on distance to card from user object


public class RelativeDisplay : MonoBehaviour
{

    public GameObject user;  //connection to object to get user position
    public GameObject card;  //connection to card object
    
    public NotecardTextEdit info;

    public string Title = "";   //default far display 
    public string LongInfo;    //close display (not used)

    float distancex;
    float distancey;
    float distancez; 
    Vector3 rangeOfView = new Vector3(2f,2f,2f);  //sets view change boundary


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //set distance every frame
        distancex = System.Math.Abs(card.transform.position.x - GetPositionR.RX);
        distancey = System.Math.Abs(card.transform.position.y - GetPositionR.RY);
        distancez = System.Math.Abs(card.transform.position.z - GetPositionR.RZ);
        string result = Title + "\n"  + LongInfo;

        //Debug.Log(distance);
         //check user distance from card
         if(distancex > rangeOfView.x || distancey > rangeOfView.y || distancez > rangeOfView.z){  //if greater than boundary
             //Debug.Log("Far");
            info.ChangeText(Title);
         }

         else{  //if close
            info.ChangeText(result);
         }

    }
}
