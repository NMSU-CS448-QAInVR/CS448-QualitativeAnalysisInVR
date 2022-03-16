using System.IO;
using UnityEngine;

public class FileManager {
    
    public static void WriteStringTo(string path, string content) {
        string final_path = Path.Combine(Application.persistentDataPath, path);
        using (StreamWriter file = new StreamWriter(final_path, false)) {
            if (file == null) {
                Debug.LogError("cannot open file - writer");
            } // end if
            file.Write(content);
            Debug.Log("wrote to file");
        } // end 
        
    } // end path

    public static string ReadStringFrom(string path) {
        string final_path = Path.Combine(Application.persistentDataPath, path);
        string result = "";
        using (StreamReader file = new StreamReader(final_path)) {
            if (file == null) {
                Debug.LogError("Cannot open file - reader");
            } // end if
            string line = "";
            while ((line = file.ReadLine()) != null) {
                result = result + line;
            } // end while

            Debug.Log("read from file");
        } // end
        
        return result;
    } // end ReadStringFrom
} // end FileManager