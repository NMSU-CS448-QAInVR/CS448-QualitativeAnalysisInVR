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
    private string current_session_path = "default_save.dat";
    
   
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

    public static string GetSessionName(string sessionPath) {
        return sessionPath.Substring(0, sessionPath.Length - 4);
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
            SaveFormat fm = item.SaveObject();
            if (fm == null) {
                Debug.Log("SaveFormat item is null");
                continue;
            } // end if
            result.Add(fm);
        } // end foreach
        FileManager.XmlSerializeList(myPath, result);
    } // end SaveOnQuest

    public List<SaveFormat> LoadFromQuest(string path) {
        string myPath = Path.Combine(session_folder, path);
        //string my_data_xml = FileManager.ReadStringFrom(myPath); 
        current_session_path = path;
        //List<SaveFormat> my_data = JsonUtility.FromJson<List<SaveFormat>>(my_data_json);
        List<SaveFormat> my_data = FileManager.XmlDeserializeList(myPath);
        return my_data;
    } // end LoadFromQuest

    public async Task<List<SaveFormat>> LoadFromQuestAsync(string path) {
        string myPath = Path.Combine(session_folder, path);
        //string my_data_xml = FileManager.ReadStringFrom(myPath); 
        current_session_path = path;
        //List<SaveFormat> my_data = JsonUtility.FromJson<List<SaveFormat>>(my_data_json);
        Debug.Log("start deserializing");
        List<SaveFormat> my_data = await FileManager.XmlDeserializeListAsync(myPath);
        Debug.Log("end deserializing");
        return my_data;
    } // end LoadFromQuest

    public List<string> GetSessionsList() {
        FileInfo[] infos = FileManager.GetFileList(session_folder);
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

    public bool DeleteSessionFile(string path) {
        try {
            string myPath = Path.Combine(session_folder, path);
            FileManager.DeleteFile(myPath);
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
