using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDocumentOnClickScript : MonoBehaviour
{
    public Object note;

    void Update() {
        Debug.Log("Hello");
    }

    public void Clicked() {
        Object.Instantiate(note, new Vector3(1, 1, 1), Quaternion.Euler(0, 0, 0));
    }
}

  
