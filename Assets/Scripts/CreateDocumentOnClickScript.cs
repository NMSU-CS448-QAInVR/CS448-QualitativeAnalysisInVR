using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDocumentOnClickScript : MonoBehaviour
{
    public Object note;

    private SaveLoadSystem sys;
    List<GameObject> objects;


    void Awake() {
        sys = new SaveLoadSystem();
        objects = new List<GameObject>();
    }
    public void Clicked() {
        Object obj = Object.Instantiate(note, new Vector3(0, 1, 0), Quaternion.Euler(0, 0, 0));
        objects.Add((GameObject) obj);
        Savable item = ((GameObject) obj).GetComponent<Savable>();
        sys.Add(item);
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

            objects.Add((GameObject) obj);
            Savable sav = ((GameObject) obj).GetComponent<Savable>();
            sys.Add(sav);
        } // end foreach
    } // end Load

    public void Delete() {
        foreach (GameObject obj in objects) {
            Object.Destroy(obj);
        } // end for each
        objects.Clear();
        sys.Clear();
    } // end Delete
}

  
