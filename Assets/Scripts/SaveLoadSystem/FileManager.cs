using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System;
using UnityEngine;

/*
    The class containing functions used to store/load files/sessions in the application. 
    Initialize() must be called before access to persistentDataPath. This is to avoid the situation where we cannot call the function to get persistentDataPath while in a user-created thread. 

*/
public class FileManager {
    // the persistent data path
    private static string persistentDataPath;

    // the types that will contain the XML saving functions. 
    private static Type[] types = {typeof(SaveFormat), typeof(NotecardSaveFormat), typeof(DrawingSaveFormat), typeof(BoardSaveFormat)};
    // The XML serializer to serialize and save the list into XML format. 
    private static XmlSerializer serializer = new XmlSerializer(typeof(ListSaveFormat), FileManager.types);
    
    /*
        A function to get and store the persistent data path. 
    */
    public static void Initialize() {
        persistentDataPath = Application.persistentDataPath;
    } // end Set PersistentDataPath

    /*
        Write a string to a path that is either absolute or relative to the persistent data path. The method to get relative path is Path.Combine() of .NET. See the API of Path.Combine() to know more. 
        Input:
        + path: The path to store the file. 
        + content: The string content to write to the file.
        + startAtPersistentDataPath: whether to add this path relative to the persistent data path or not.
    */
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

    /*
        Delete a file with the path that is either absolute or relative to the persistent data path. The method to get relative path is Path.Combine() of .NET. See the API of Path.Combine() to know more. 
        Input:
        + path: The path to the file to delete.
        + startAtPersistentDataPath: whether to add this path relative to the persistent data path or not.
    */
    public static void DeleteFile(string path, bool startAtPersistentPath=true) {
        string final_path = path;
        if (startAtPersistentPath) {
            final_path = Path.Combine(persistentDataPath, path);
        } // end if
        File.Delete(final_path);
    } // end DeleteFile

    /*
        XML Serialize a list of SaveFormat into the path relative to the persistent data path. The method to get relative path is Path.Combine() of .NET. See the API of Path.Combine() to know more.
        Input:
        + path: the relative path to the persistent data path to save the list into. 
        + list: the list of SaveFormat to save. 

        SaveFormat is from Scripts/SaveLoadSystem/SaveFormat
    */
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

    /*
        An async version to XML serialize a list of SaveFormat into the path relative to the persistent data path. The method to get relative path is Path.Combine() of .NET. See the API of Path.Combine() to know more.
        Input:
        + path: the relative path to the persistent data path to save the list into. 
        + list: the list of SaveFormat to save.

        SaveFormat is from Scripts/SaveLoadSystem/SaveFormat
    */
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

    /*
        Read the entire string content from a path that is either absolute or relative to the persistent data path. The method to get relative path is Path.Combine() of .NET. See the API of Path.Combine() to know more. 
        Input:
        + path: The path to read the file. 
        + startAtPersistentDataPath: whether to add this path relative to the persistent data path or not.

        Output: The entire string content from the file at path. 
    */
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

    /*
        An async version to read the entire string content from a path that is either absolute or relative to the persistent data path. The method to get relative path is Path.Combine() of .NET. See the API of Path.Combine() to know more. 
        Input:
        + path: The path to read the file. 
        + startAtPersistentDataPath: whether to add this path relative to the persistent data path or not.

        Output: The entire string content from the file at path. 
    */
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

    /*
        Read all bytes from a path that is either absolute or relative to the persistent data path. The method to get relative path is Path.Combine() of .NET. See the API of Path.Combine() to know more. 
        Input:
        + path: The path to read the file. 
        + startAtPersistentDataPath: whether to add this path relative to the persistent data path or not.

        Output: an array of all bytes from the file at path. 
    */
    public static byte[] ReadBytesFrom(string path, bool startAtPersistentPath=true) {
        string final_path = path;
        if (startAtPersistentPath) {
            final_path = Path.Combine(persistentDataPath, path);
        } // end if
        byte[] result = null;
        result = File.ReadAllBytes(final_path);
       
        return result;
    } // end ReadStringFrom

    /*
        An async function to read all bytes from a path that is either absolute or relative to the persistent data path. The method to get relative path is Path.Combine() of .NET. See the API of Path.Combine() to know more. 
        Input:
        + path: The path to read the file. 
        + startAtPersistentDataPath: whether to add this path relative to the persistent data path or not.

        Output: an array of all bytes from the file at path. 
    */
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

    /*
        An async function to write bytes to a path that is either absolute or relative to the persistent data path. The method to get relative path is Path.Combine() of .NET. See the API of Path.Combine() to know more. 
        Input:
        + path: The path to write the bytes into. 
        + startAtPersistentDataPath: whether to add this path relative to the persistent data path or not.

        Output: A task to 'await' on. 
    */
    public static async Task WriteBytesToAsync(string path, byte[] bytes, bool startAtPersistentPath=true) {
        string final_path = path;
        if (startAtPersistentPath) {
            final_path = Path.Combine(persistentDataPath, path);
        } // end if
        await Task.Run(() => {
            File.WriteAllBytes(final_path, bytes);
        });
    } // end ReadStringFrom

    /*
        An async function to check if a file exists at path, which is either absolute or relative to the persistent data path. The method to get relative path is Path.Combine() of .NET. See the API of Path.Combine() to know more. 
        Input:
        + path: The path to check if a file exists there. 
        + startAtPersistentDataPath: whether to add this path relative to the persistent data path or not.

        Output: Return true if a file exists at path, else false.
    */
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

    /*
        An async function to check if a directory exists at path, which is either absolute or relative to the persistent data path. The method to get relative path is Path.Combine() of .NET. See the API of Path.Combine() to know more. 
        Input:
        + path: The path to check if a directory exists there. 
        + startAtPersistentDataPath: whether to add this path relative to the persistent data path or not.

        Output: Return true if a directory exists at path, else false.
    */
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

    /*
        A function to get a list of information about the files that are in the directory at path, which is either absolute or relative to the persistent data path. The method to get relative path is Path.Combine() of .NET. See the API of Path.Combine() to know more. 
        Input:
        + path: The path of the directory to get the list of files. 
        + startAtPersistentDataPath: whether to add this path relative to the persistent data path or not.

        Output: Return an array of FileInfo containing the information about the files that are in the directory at path. 
        FileInfo is a built-in class of .NET
    */
    public static FileInfo[] GetFileList(string path = "", bool startAtPersistentPath=true) {
        string final_path = path;
        if (startAtPersistentPath) {
            final_path = Path.Combine(persistentDataPath, path);
        } // end if
        DirectoryInfo di = new DirectoryInfo(final_path);
        return di.GetFiles();
    } // end GetFileList
    
    /*
        A function to get a list of information about the subdirectories that are in the directory at path, which is either absolute or relative to the persistent data path. The method to get relative path is Path.Combine() of .NET. See the API of Path.Combine() to know more. 
        Input:
        + path: The path of the directory to get the list of subdirectories. 
        + startAtPersistentDataPath: whether to add this path relative to the persistent data path or not.

        Output: Return an array of DirectoryInfo containing the information about the subdirectories that are in the directory at path. 
        DirectoryInfo is a built-in class of .NET
    */
    public static DirectoryInfo[] GetDirsList(string path = "", bool startAtPersistentPath=true) {
        string final_path = path;
        if (startAtPersistentPath) {
            final_path = Path.Combine(persistentDataPath, path);
        } // end if
        DirectoryInfo di = new DirectoryInfo(final_path);
        return di.GetDirectories();
    } // end GetFileList

    /*
        A function to check if a string ends with another string. 
        Input: 
        + str: The string to check if ends with something. 
        + end: The string to check if str ends with it. 

        Output: True if "str" ends with "end", else False. 
    */
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

    /*
        XML Deserialize from a file to get a list of SaveFormat at the path relative to the persistent data path. The method to get relative path is Path.Combine() of .NET. See the API of Path.Combine() to know more.
        Input:
        + path: the relative path to the persistent data path to get the list from. 

        Output: 
        + A list of SaveFormat from the file at path with XML content. 

        SaveFormat is from Scripts/SaveLoadSystem/SaveFormat
    */
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

    /*
        An async function to XML Deserialize from a file to get a list of SaveFormat at the path relative to the persistent data path. The method to get relative path is Path.Combine() of .NET. See the API of Path.Combine() to know more.
        Input:
        + path: the relative path to the persistent data path to get the list from. 

        Output: 
        + A list of SaveFormat from the file at path with XML content. 

        SaveFormat is from Scripts/SaveLoadSystem/SaveFormat
    */
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

    /*
        Unimplemented/not-used functions. Meant to get the name of the file or directory from the path
    */
    public static string GetNameFromPath(string path) {
        // to be done
        return "";
    } // end GetNameFromPath

    /*
        A function to get the information about the files and subdirectories that are in the directory at path, which is either absolute or relative to the persistent data path. The method to get relative path is Path.Combine() of .NET. See the API of Path.Combine() to know more. 
        Input:
        + path: The path of the directory to get the list of files and subdirectories. 
        + startAtPersistentDataPath: whether to add this path relative to the persistent data path or not.

        Output: Return a DirectoryInfo containing the information about the files and subdirectories that are in the directory at path. 
        DirectoryInfo is a built-in class of .NET
    */
    public static DirectoryInfo GetFilesAndDirsAt(string path, bool startAtPersistentPath=true) {
        string final_path = path;
        if (startAtPersistentPath) {
            final_path = Path.Combine(persistentDataPath, path);
        } // end if
        DirectoryInfo di = new DirectoryInfo(final_path);
        return di;
    } // end GetFilesAndDirsAt

    /*
        Get the persistent data path of the application
        Output: A string representing the persistent data path of the application
    */
    public static string GetDataPath() {
        return persistentDataPath;
    } // end GetDataPath

    /*
        A function to create a directory at path, which is either absolute or relative to the persistent data path. The method to get relative path is Path.Combine() of .NET. See the API of Path.Combine() to know more. 
        Input:
        + path: The path of the directory to create the directory at.
        + startAtPersistentDataPath: whether to add this path relative to the persistent data path or not.
    */
    public static void CreateDirectory(string path, bool startAtPersistentPath=true) {
        string myPath = path;
        if (startAtPersistentPath) {
            myPath = Path.Combine(persistentDataPath, path);
        } // end if
        Directory.CreateDirectory(myPath);
    } // end path

    /*
        A function to delete a directory at path, which is either absolute or relative to the persistent data path. The method to get relative path is Path.Combine() of .NET. See the API of Path.Combine() to know more. 
        Input:
        + path: The path of the directory to delete.
        + startAtPersistentDataPath: whether to add this path relative to the persistent data path or not.
    */
    public static void DeleteDirectoryRecursive(string path, bool startAtPersistentPath=true) {
        string myPath = path;
        if (startAtPersistentPath) {
            myPath = Path.Combine(persistentDataPath, path);
        } // end if
        Directory.Delete(myPath, true);
    } // end path
} // end FileManager