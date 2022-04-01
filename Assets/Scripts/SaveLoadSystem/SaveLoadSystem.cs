using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveLoadSystem
{
    List<Savable> items;
    public List<GameObject> objects;
    private string current_path = "default_save.dat";
   
    public SaveLoadSystem() {
        items = new List<Savable>();
        objects = new List<GameObject>();
    } // end Awake

    public void Add(GameObject obj) {
        if (obj == null)
            return;
        
        objects.Add(obj);
        Savable sav = ((GameObject) obj).GetComponent<Savable>();
        items.Add(sav);
    } // end Add

    public void Remove(GameObject obj) {
        if (obj == null)
            return;
        
        objects.Remove(obj);
        Savable sav = ((GameObject) obj).GetComponent<Savable>();
        items.Remove(sav);
    } // end Remove

    public void Clear() {
        objects.Clear();
        items.Clear();
    } // end Clear

    public void SaveOnQuest(string path, bool setCurrentPath=false) {
        if (setCurrentPath) {
            current_path = path;
        } // end if

        List<SaveFormat> result = new List<SaveFormat>();
        foreach (Savable item in items) {
            SaveFormat fm = item.SaveObject();
            if (fm == null) {
                Debug.Log("SaveFormat item is null");
                continue;
            } // end if
            result.Add(fm);
        } // end foreach
        Debug.Log("Count is " + result.Count);
        FileManager.XmlSerializeList(path, result);
    } // end SaveOnQuest

    public List<SaveFormat> LoadFromQuest(string path) {
        string my_data_xml = FileManager.ReadStringFrom(path); 
        current_path = path;
        Debug.Log(my_data_xml);
        //List<SaveFormat> my_data = JsonUtility.FromJson<List<SaveFormat>>(my_data_json);
        List<SaveFormat> my_data = FileManager.XmlDeserializeList(path);
        return my_data;
    } // end LoadFromQuest

    public List<string> GetSessionsList() {
        FileInfo[] infos = FileManager.GetFileList();
        Debug.Log("There are  " + infos.Length + " files");
        List<string> result = new List<string>();
        if (infos != null) {
            foreach (FileInfo info in infos) {
                Debug.Log("In file: " + info.Name);
                string name = info.Name;
                if (FileManager.EndsWith(name, ".dat")) {
                    result.Add(name);
                } // end if
            } // end foreach
        } // end if
        return result;
    } // end GetSessionsList

    public string GetCurrentPath() {
        return current_path;
    } // end GetCurrentPath
}
