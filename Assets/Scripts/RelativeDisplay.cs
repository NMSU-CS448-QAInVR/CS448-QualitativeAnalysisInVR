using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RelativeDisplay : MonoBehaviour
{

    public GameObject user;
    public GameObject card;
    
    public NotecardTextEdit info;

    public string Title = "";
    public string LongInfo;

    float distancex;
    float distancey;
    float distancez; 
    Vector3 rangeOfView = new Vector3(2f,2f,2f);


    // Start is called before the first frame update
    void Start()
    {
        //distance = card.transform.position - user.transform.position;
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
         
         if(distancex > rangeOfView.x || distancey > rangeOfView.y || distancez > rangeOfView.z){
             //Debug.Log("Far");
            info.ChangeText(Title);
         }

         else{
            info.ChangeText(result);
         }

    }
}
