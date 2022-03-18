using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveLoadSystem
{
    List<Savable> items;
    private const string path = "test_save.dat";

    public SaveLoadSystem() {
        items = new List<Savable>();
    } // end Awake

    public void Add(Savable obj) {
        if (obj == null)
            return;
        
        items.Add(obj);
    } // end Add

    public void Remove(Savable obj) {
        if (obj == null)
            return;
        
        items.Remove(obj);
    } // end Remove

    public void Clear() {
        items.Clear();
    } // end Clear

    public void SaveOnQuest() {
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

    public List<SaveFormat> LoadFromQuest() {
        string my_data_xml = FileManager.ReadStringFrom(path); 
        Debug.Log(my_data_xml);
        //List<SaveFormat> my_data = JsonUtility.FromJson<List<SaveFormat>>(my_data_json);
        List<SaveFormat> my_data = FileManager.XmlDeserializeList(path);
        return my_data;
    }
}
