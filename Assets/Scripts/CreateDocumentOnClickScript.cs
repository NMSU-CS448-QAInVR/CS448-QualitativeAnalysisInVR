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

    public void Save() {
        sys.SaveOnQuest(sys.GetCurrentPath());
    } // end Save

    public void SaveAs(string path) {
         sys.SaveOnQuest(path, true);
    } // end SaveAs

    public void Load(string path) {
        Delete();
        List<SaveFormat> items = sys.LoadFromQuest(path);
        if (items == null) {
            Debug.LogError("The loaded items is empty");
            return;
        } // end if

        foreach (SaveFormat item in items) {
            GameObject obj = (GameObject) Object.Instantiate(note, new Vector3(0, 1, 0), Quaternion.Euler(0, 0, 0));
            Debug.Log(item);
            item.LoadObjectInto(obj);

            sys.Add(obj);
        } // end foreach
    } // end Load

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

  
