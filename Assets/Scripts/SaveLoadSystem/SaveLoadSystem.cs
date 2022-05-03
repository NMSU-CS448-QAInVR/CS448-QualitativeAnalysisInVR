using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class SaveLoadSystem
{
    List<Savable> items;
    public List<GameObject> objects;
    private string session_folder = "sessions/";
    private string current_session_path = "default_save";
    
   
    public SaveLoadSystem() {
        items = new List<Savable>();
        objects = new List<GameObject>();
    } // end Awake

    public void Initialize() {
        FileManager.CreateDirectory(session_folder);
    } // end Initialize

    public void Add(GameObject obj) {
        if (obj == null)
            return;
        
        objects.Add(obj);
        Savable sav = obj.GetComponent<Savable>();
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
        foreach (Savable item in items) {
            item.DeleteSelf();
        } // end for each
        objects.Clear();
        items.Clear();
        Debug.Log(items.Count);
        Debug.Log(objects.Count);
    } // end Clear

    public static string GetSessionName(string sessionPath) {
        return sessionPath;
    } // end GetSessionName

    public void AddExternalStuffs(List<Savable> externalSavable) {
        IEnumerable<Savable> ie = externalSavable;
        items.AddRange(ie);
        objects.AddRange(ie.Select((Savable sav) => sav.gameObject));
    } // end AddExternalSavable

    public void SaveOnQuest(string path, bool setCurrentPath=false) {
        string myPath = Path.Combine(session_folder, path);
        if (setCurrentPath) {
            current_session_path = path;
        } // end if

        List<SaveFormat> result = new List<SaveFormat>();
        foreach (Savable item in items) {
            SaveFormat fm = item.SaveObject(session_folder).Result;
            if (fm == null) {
                Debug.Log("SaveFormat item is null");
                continue;
            } // end if
            result.Add(fm);
        } // end foreach
        FileManager.XmlSerializeList(myPath, result);
    } // end SaveOnQuest

    public async Task SaveOnQuestAsync(string path, bool setCurrentPath=false) {
        string myPath = Path.Combine(session_folder, path);
        if (setCurrentPath) {
            current_session_path = path;
        } // end if

        if (! await FileManager.ThisDirectoryExists(myPath)) {
            FileManager.CreateDirectory(myPath);
        } // end if
        
        NotecardSaveFormat.ResetNotecardNo();
        string data_file_path = Path.Combine(myPath, path + ".dat");
        
        List<SaveFormat> result = new List<SaveFormat>();
        foreach (Savable item in items) {
            SaveFormat fm = await item.SaveObject(myPath);
            if (fm == null) {
                //throw new Exception("SaveFormat item is null");
                continue;
            } // end if
            result.Add(fm);
        } // end foreach

        await FileManager.XmlSerializeListAsync(data_file_path, result);
    } // end SaveOnQuest

    public List<SaveFormat> LoadFromQuest(string path) {
        string myPath = Path.Combine(session_folder, path);
        string data_file_path = Path.Combine(myPath, path + ".dat");
        //string my_data_xml = FileManager.ReadStringFrom(myPath); 
        current_session_path = path;
        //List<SaveFormat> my_data = JsonUtility.FromJson<List<SaveFormat>>(my_data_json);
        List<SaveFormat> my_data = FileManager.XmlDeserializeList(data_file_path);
        return my_data;
    } // end LoadFromQuest

    public async Task<List<SaveFormat>> LoadFromQuestAsync(string path) {
        // Debug.Log("Having " + objects.Count + " objects.");
        // Debug.Log("Having " + items.Count + " savables.");
        string myPath = Path.Combine(session_folder, path);
        string data_file_path = Path.Combine(myPath, path + ".dat");
        current_session_path = path;
        //Debug.Log("Load from: " + data_file_path);
        List<SaveFormat> my_data = await FileManager.XmlDeserializeListAsync(data_file_path);
        return my_data;
    } // end LoadFromQuest

    public List<string> GetSessionsList() {
        DirectoryInfo[] infos = FileManager.GetDirsList(session_folder);
        List<string> result = new List<string>();
        if (infos != null) {
            foreach (DirectoryInfo info in infos) {
                string name = info.Name;
                result.Add(name);
            } // end foreach
        } // end if
        return result;
    } // end GetSessionsList

    public bool DeleteSessionFile(string path) {
        try {
            string myPath = Path.Combine(session_folder, path);
            FileManager.DeleteDirectoryRecursive(myPath);
            return true;
        } catch (SystemException ex) {
            Debug.LogError(ex.Message);
            return false;
        } // end catch        
    } // end DeleteSessionFile

    public string GetCurrentPath() {
        return current_session_path;
    } // end GetCurrentPath
}
