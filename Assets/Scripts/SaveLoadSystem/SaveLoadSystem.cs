using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

/*
    A class for objects to handle the saving and loading of sessions.
    An object of this class will also keep track of the recently opened session. 
    Savable can be found at Scripts/SaveLoadSystem/Savable.

    Initialize() should be called before saving/loading to make sure that the folder containing the folders of each session is created. 
*/
public class SaveLoadSystem
{   
    // A list of Savable of to-be-saved items added with the Add() function. The only items are notecards right now. 
    List<Savable> items;
    // A list of Savable for boards to be saved. The Savable of boards are not destroyed as the current implementation of the app do not create more or destroy boards. 
    List<Savable> boards;
    // A list of Savable of drawings. 
    List<Savable> drawings;
    // A list of GameObjects corresponding to the list "items". 
    public List<GameObject> objects;
    // The session where the folders of each session is stored. 
    private string session_folder = "sessions/";
    // The default session to save to when no other session has been loaded when the app first load. 
    private string current_session_path = "default_save";
    
    public SaveLoadSystem() {
        items = new List<Savable>();
        objects = new List<GameObject>();
        boards = new List<Savable>();
        drawings = new List<Savable>();
    } // end Awake

    /*
        Create the root folder of all sessions. 
    */
    public void Initialize() {
        FileManager.CreateDirectory(session_folder);
    } // end Initialize

    /*
        Add a Savable object to be saved. 
        Input: 
        + obj: the object to add.
    */
    public void Add(GameObject obj) {
        if (obj == null)
            return;
        
        objects.Add(obj);
        Savable sav = obj.GetComponent<Savable>();
        items.Add(sav);
    } // end Add

    /*
        This is currently not used. 
        Remove a Savable object from the items. 
        Input:
        + obj: the object to remove
    */
    public void Remove(GameObject obj) {
        if (obj == null)
            return;
        
        objects.Remove(obj);
        Savable sav = ((GameObject) obj).GetComponent<Savable>();
        items.Remove(sav);
    } // end Remove

    /*
        Destroy the to-be-saved objects and clear the list containing them. 
    */
    public void Clear() {
        // we ignore every exceptions happened when destroying the objects. 
        try {
            // items
            foreach (Savable item in items) {
                item.DeleteSelf();
            } // end for each

            // drawings
            foreach (Savable drawing in drawings) {
                drawing.DeleteSelf();
            } // end foreach

            // boards
            foreach (Savable board in boards) {
                board.DeleteSelf();
            } // end foreach
        } catch (Exception e) {
            Debug.LogError(e.Message);
            Debug.LogError(e.StackTrace);
        } // end catch
      
        objects.Clear();
        items.Clear();
        drawings.Clear();
    } // end Clear

    /*
        Meant to get the session name from the session path. However, in the current implementation, the session path starts from the session folder, so it is the session name by itself. 
    */
    public static string GetSessionName(string sessionPath) {
        return sessionPath;
    } // end GetSessionName

    /*
        Add a list of Savable of all of the drawings.
    */
    public void AddDrawings(List<Savable> _drawings) {
        IEnumerable<Savable> ie = _drawings;
        this.drawings.AddRange(ie);
    } // end AddExternalSavable

    /*
        Add a list of Savable of all of the boards.
    */
    public void AddBoards(List<Savable> _boards) {
        IEnumerable<Savable> ie = _boards;
        this.boards.AddRange(ie);
    } // end AddBoards

    /*
        Save all of the current objects to a session name called path.
        Input:
        + path: the name of the session
        + setCurrentPath: set this session as a current session or not
    */
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

        // add boards
        foreach (Savable board in boards) {
            SaveFormat fm = board.SaveObject(session_folder).Result;
            if (fm == null) {
                Debug.Log("SaveFormat item is null");
                continue;
            } // end if
            result.Add(fm);
        } // end foreach

        // add drawings
        foreach (Savable drawing in drawings) {
            SaveFormat fm = drawing.SaveObject(session_folder).Result;
            if (fm == null) {
                Debug.Log("SaveFormat item is null");
                continue;
            } // end if
            result.Add(fm);
        } // end foreach

        FileManager.XmlSerializeList(myPath, result);
    } // end SaveOnQuest

    /*
        An async function to save all of the current objects to a session name called path.
        Input:
        + path: the name of the session
        + setCurrentPath: set this session as a current session or not
    */
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

        // add boards
        foreach (Savable board in boards) {
            SaveFormat fm = await board.SaveObject(myPath);
            if (fm == null) {
                //throw new Exception("SaveFormat item is null");
                continue;
            } // end if
            result.Add(fm);
        } // end foreach

        Debug.Log("Before drawing: " + drawings.Count);
         // add drawings
        foreach (Savable drawing in drawings) {
            Debug.Log("In Drawing");
            SaveFormat fm = await drawing.SaveObject(myPath);
            if (fm == null) {
                Debug.Log("Drawing is null");
                //throw new Exception("SaveFormat item is null");
                continue;
            } // end if
            Debug.Log("Save drawing");
            result.Add(fm);
        } // end foreach

        await FileManager.XmlSerializeListAsync(data_file_path, result);
    } // end SaveOnQuest

    /*
        Get a list of SaveFormat of the objects in the session called path.
        Input:
        + path: the name of the session

        Output: A list of SaveFormat containing the objects in the session called path.
        SaveFormat is in Scripts/SaveLoadSystem/Savable/SaveFormat
    */
    public List<SaveFormat> LoadFromQuest(string path) {
        string myPath = Path.Combine(session_folder, path);
        string data_file_path = Path.Combine(myPath, path + ".dat");
        //string my_data_xml = FileManager.ReadStringFrom(myPath); 
        current_session_path = path;
        //List<SaveFormat> my_data = JsonUtility.FromJson<List<SaveFormat>>(my_data_json);
        List<SaveFormat> my_data = FileManager.XmlDeserializeList(data_file_path);
        return my_data;
    } // end LoadFromQuest

    /*
        An async function to get a list of SaveFormat of the objects in the session called path.
        Input:
        + path: the name of the session

        Output: A list of SaveFormat containing the objects in the session called path.
        SaveFormat is in Scripts/SaveLoadSystem/Savable/SaveFormat
    */
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

    /*
        Get a list of names of all of the sessions stored in the application persistent data path.
    */
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

    /*
        Delete a session with the name path.
        Input:
        + path: the name of the session to delete. 

        Output: return true if deleted successfully, else false. 
    */
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

    /*
        Get the current session name
    */
    public string GetCurrentPath() {
        return current_session_path;
    } // end GetCurrentPath
}
