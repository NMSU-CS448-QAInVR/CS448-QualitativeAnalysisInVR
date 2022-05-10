using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDocumentOnClickScript : MonoBehaviour
{
    public Object note;

    private SaveLoadSystem sys;
   


    void Awake() {
        sys = new SaveLoadSystem();
        FileManager.Initialize();
    }
    public void Clicked() {
        Object obj = Object.Instantiate(note, new Vector3(0, 1, 0), Quaternion.Euler(0, 0, 0));
        sys.Add((GameObject) obj);
    }

    public void Delete() {
        foreach (GameObject obj in sys.objects) {
            Object.Destroy(obj);
        } // end for each
        sys.Clear();
    } // end Delete

    public void DeleteASession(string session_name) {
        FileManager.DeleteFile(session_name);
    } // end DeleteASession
}

  
