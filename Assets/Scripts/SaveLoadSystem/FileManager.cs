using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System;
using UnityEngine;

public class FileManager {
    private static string persistentDataPath;


    private static Type[] types = {typeof(SaveFormat), typeof(NotecardSaveFormat)};
    private static XmlSerializer serializer = new XmlSerializer(typeof(ListSaveFormat), FileManager.types);
    
    public static void Initialize() {
        persistentDataPath = Application.persistentDataPath;
    } // end Set PersistentDataPath

    public static void WriteStringTo(string path, string content) {
        string final_path = Path.Combine(persistentDataPath, path);
        using (StreamWriter file = new StreamWriter(final_path, false)) {
            if (file == null) {
                Debug.LogError("cannot open file - writer");
            } // end if
            
            file.Write(content);
            Debug.Log("wrote to file");
        } // end 
        
    } // end path

    public static void DeleteFile(string path) {
        string final_path = Path.Combine(persistentDataPath, path);
        File.Delete(final_path);
    } // end DeleteFile

    public static void XmlSerializeList(string path, List<SaveFormat> list) {
        
        string final_path = Path.Combine(persistentDataPath, path);
        using (StreamWriter file = new StreamWriter(final_path, false)) {
            if (file == null) {
                Debug.LogError("cannot open file - writer");
            } // end if

            ListSaveFormat list_save = new ListSaveFormat();
            list_save.list_format = list;
            
            serializer.Serialize(file.BaseStream, list_save);
            Debug.Log("wrote to file");
        } // end 
    } // end SaveListFormat

    public static string ReadStringFrom(string path) {
        string final_path = Path.Combine(persistentDataPath, path);
        string result = "";
        using (StreamReader file = new StreamReader(final_path)) {
            if (file == null) {
                Debug.LogError("Cannot open file - reader");
            } // end if
            string line = "";
            while ((line = file.ReadLine()) != null) {
                result = result + line + "\n";
            } // end while

            Debug.Log("read from file");
        } // end
        
        return result;
    } // end ReadStringFrom

    public static FileInfo[] GetFileList() {
        DirectoryInfo di = new DirectoryInfo(persistentDataPath);
        return di.GetFiles();
    } // end GetFileList

    public static bool EndsWith(string str, string end) {
        int strLen = str.Length;
        for (int i = 0; i < end.Length; ++i) {
            int strIdx = strLen - 1 - i;
            int endIdx = end.Length - 1 - i;
            if (strIdx < str.Length && str[strIdx] != end[endIdx]) {
                return false;
            } // end if
        } // end for i

        return true;
    } // end EndsWith

    public static List<SaveFormat> XmlDeserializeList(string path) {
        string final_path = Path.Combine(persistentDataPath, path);
        using (StreamReader file = new StreamReader(final_path)) {
            if (file == null) {
                Debug.LogError("Cannot open file - reader");
            } // end if
            
            ListSaveFormat save_list = (ListSaveFormat) serializer.Deserialize(file.BaseStream);
            Debug.Log("Deserialize Xml from file");
            return save_list.list_format;
        } // end
    } // end Des

    public static string GetNameFromPath(string path) {
        // to be done
        return "";
    } // end GetNameFromPath

    public static DirectoryInfo GetFilesAndDirsAt(string path) {
        DirectoryInfo di = new DirectoryInfo(path);
        return di;
    } // end GetFilesAndDirsAt

    public static string GetDataPath() {
        return persistentDataPath;
    } // end GetDataPath
} // end FileManager