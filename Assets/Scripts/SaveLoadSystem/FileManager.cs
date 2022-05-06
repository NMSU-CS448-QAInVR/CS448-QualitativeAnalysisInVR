using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System;
using UnityEngine;

public class FileManager {
    private static string persistentDataPath;


    private static Type[] types = {typeof(SaveFormat), typeof(NotecardSaveFormat), typeof(DrawingSaveFormat), typeof(BoardSaveFormat)};
    private static XmlSerializer serializer = new XmlSerializer(typeof(ListSaveFormat), FileManager.types);
    
    public static void Initialize() {
        persistentDataPath = Application.persistentDataPath;
    } // end Set PersistentDataPath

    public static void WriteStringTo(string path, string content, bool startAtPersistentPath=true) {
        string final_path = path;
        if (startAtPersistentPath) {
            final_path = Path.Combine(persistentDataPath, path);
        } // end if
        
        using (StreamWriter file = new StreamWriter(final_path, false)) {
            if (file == null) {
                Debug.LogError("cannot open file - writer");
            } // end if
            
            file.Write(content);
            Debug.Log("wrote to file");
        } // end 
        
    } // end path

    public static void DeleteFile(string path, bool startAtPersistentPath=true) {
        string final_path = path;
        if (startAtPersistentPath) {
            final_path = Path.Combine(persistentDataPath, path);
        } // end if
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

     public static async Task XmlSerializeListAsync(string path, List<SaveFormat> list) {
        
        string final_path = Path.Combine(persistentDataPath, path);
        using (StreamWriter file = new StreamWriter(final_path, false)) {
            if (file == null) {
                Debug.LogError("cannot open file - writer");
            } // end if

            ListSaveFormat list_save = new ListSaveFormat();
            list_save.list_format = list;
            
            await Task.Run(() => {
                serializer.Serialize(file.BaseStream, list_save);
            });
            Debug.Log("wrote to file");
        } // end 
    } // end SaveListFormat

    public static string ReadStringFrom(string path, bool startAtPersistentPath=true) {
        string final_path = path;
        if (startAtPersistentPath) {
            final_path = Path.Combine(persistentDataPath, path);
        } // end if
        string result = "";
        using (StreamReader file = new StreamReader(final_path)) {
            if (file == null) {
                Debug.LogError("Cannot open file - reader");
            } // end if
            string line = "";
            while ((line = file.ReadLine()) != null) {
                result = result + line+'\n';
            } // end while

            Debug.Log("read from file");
        } // end
        
        return result;
    } // end ReadStringFrom

    public static async Task<string> ReadStringFromAsync(string path, bool startAtPersistentPath=true) {
        string final_path = path;
        if (startAtPersistentPath) {
            final_path = Path.Combine(persistentDataPath, path);
        } // end if
        string result = "";
        using (StreamReader file = new StreamReader(final_path)) {
            if (file == null) {
                Debug.LogError("Cannot open file - reader");
            } // end if
            string line = "";
            await Task.Run(() => {
                while ((line = file.ReadLine()) != null) {
                    result = result + line + "\n";
                } // end while
            });
        } // end
        
        return result;
    } // end ReadStringFrom

    public static byte[] ReadBytesFrom(string path, bool startAtPersistentPath=true) {
        string final_path = path;
        if (startAtPersistentPath) {
            final_path = Path.Combine(persistentDataPath, path);
        } // end if
        byte[] result = null;
        result = File.ReadAllBytes(final_path);
       
        return result;
    } // end ReadStringFrom

    public static async Task<byte[]> ReadBytesFromAsync(string path, bool startAtPersistentPath=true) {
        string final_path = path;
        if (startAtPersistentPath) {
            final_path = Path.Combine(persistentDataPath, path);
        } // end if
        byte[] result = null;
        await Task.Run(() => {
            result = File.ReadAllBytes(final_path);
        });
       
        return result;
    } // end ReadStringFrom

    public static async Task WriteBytesToAsync(string path, byte[] bytes, bool startAtPersistentPath=true) {
        string final_path = path;
        if (startAtPersistentPath) {
            final_path = Path.Combine(persistentDataPath, path);
        } // end if
        await Task.Run(() => {
            File.WriteAllBytes(final_path, bytes);
        });
    } // end ReadStringFrom

    public static async Task<bool> ThisFileExists(string path, bool startAtPersistentPath=true) {
        string final_path = path;
        if (startAtPersistentPath) {
            final_path = Path.Combine(persistentDataPath, path);
        } // end if
        bool result = false;
        await Task.Run(() => {
            result = File.Exists(path);
        });
        return result;
    } // end ThisFileExists

    public static async Task<bool> ThisDirectoryExists(string path, bool startAtPersistentPath=true) {
        string final_path = path;
        if (startAtPersistentPath) {
            final_path = Path.Combine(persistentDataPath, path);
        } // end if
        bool result = false;
        await Task.Run(() => {
            result = Directory.Exists(path);
        });
        return result;
    }

    public static FileInfo[] GetFileList(string path = "", bool startAtPersistentPath=true) {
        string final_path = path;
        if (startAtPersistentPath) {
            final_path = Path.Combine(persistentDataPath, path);
        } // end if
        DirectoryInfo di = new DirectoryInfo(final_path);
        return di.GetFiles();
    } // end GetFileList

    public static DirectoryInfo[] GetDirsList(string path = "", bool startAtPersistentPath=true) {
        string final_path = path;
        if (startAtPersistentPath) {
            final_path = Path.Combine(persistentDataPath, path);
        } // end if
        DirectoryInfo di = new DirectoryInfo(final_path);
        return di.GetDirectories();
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
            
            //ListSaveFormat save_list = (ListSaveFormat) serializer.Deserialize(file.BaseStream);
            ListSaveFormat save_list = new ListSaveFormat();
            Debug.Log("Deserialize Xml from file");
            return save_list.list_format;
        } // end
    } // end Des

    public static async Task<List<SaveFormat>> XmlDeserializeListAsync(string path) {
        string final_path = Path.Combine(persistentDataPath, path);
        using (StreamReader file = new StreamReader(final_path)) {
            if (file == null) {
                Debug.LogError("Cannot open file - reader");
            } // end if
            
            ListSaveFormat save_list = null;
            await Task.Run(() => {
                save_list = (ListSaveFormat) serializer.Deserialize(file.BaseStream);
            });
            Debug.Log("Deserialize Xml from file");
            return save_list.list_format;
        } // end
    } // end Des

    public static string GetNameFromPath(string path) {
        // to be done
        return "";
    } // end GetNameFromPath

    public static DirectoryInfo GetFilesAndDirsAt(string path, bool startAtPersistentPath=true) {
        string final_path = path;
        if (startAtPersistentPath) {
            final_path = Path.Combine(persistentDataPath, path);
        } // end if
        DirectoryInfo di = new DirectoryInfo(final_path);
        return di;
    } // end GetFilesAndDirsAt

    public static string GetDataPath() {
        return persistentDataPath;
    } // end GetDataPath

    public static void CreateDirectory(string path, bool startAtPersistentPath=true) {
        string myPath = path;
        if (startAtPersistentPath) {
            myPath = Path.Combine(persistentDataPath, path);
        } // end if
        Directory.CreateDirectory(myPath);
    } // end path

    public static void DeleteDirectoryRecursive(string path, bool startAtPersistentPath=true) {
        string myPath = path;
        if (startAtPersistentPath) {
            myPath = Path.Combine(persistentDataPath, path);
        } // end if
        Directory.Delete(myPath, true);
    } // end path
} // end FileManager