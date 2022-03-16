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


    public string GetJsonSave() {
        List<SaveFormat> result = new List<SaveFormat>();
        foreach (Savable item in items) {
            result.Add(item.SaveObject());
        } // end foreach
        string my_data_json = JsonUtility.ToJson(items);
        return my_data_json;
    } // end Save

    public void SaveOnQuest() {
        string my_data = GetJsonSave();
        FileManager.WriteStringTo(path, my_data);
    } // end SaveOnQuest

    public List<SaveFormat> LoadFromQuest() {
        string my_data_json = FileManager.ReadStringFrom(path); 
        List<SaveFormat> my_data = JsonUtility.FromJson<List<SaveFormat>>(my_data_json);
        return my_data;
    }
}
