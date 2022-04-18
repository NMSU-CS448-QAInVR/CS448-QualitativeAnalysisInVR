using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snapOnCollision : MonoBehaviour
{

    public GameObject noteCard;
    

    // Start is called before the first frame update
       void OnCollisionEnter(Collision collision) {
        //might need to check for if cardHeld is false also
          //f(noteCard.cardHeld == false){
          noteCard.transform.position = new Vector3(transform.position.x - .1f, noteCard.transform.position.y, noteCard.transform.position.z);;
         // this.GetComponent<Renderer> ().material.color = Color.white;
          //}// if letgo
          //else{

           // this.GetComponent<Renderer> ().material.color = Color.green;

         // }
         Debug.Log("Collided with board");
    } // end OnCollisionEnter
}
