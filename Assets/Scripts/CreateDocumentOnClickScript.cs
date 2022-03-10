using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDocumentOnClickScript : MonoBehaviour
{
    public Object note;

    public void Clicked() {
        Object.Instantiate(note, new Vector3(0, 1, 0), Quaternion.Euler(0, 0, 0));
        Debug.Log("Hello");
    }
}

  
