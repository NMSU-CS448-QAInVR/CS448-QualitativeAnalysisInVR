using UnityEngine;
using System;

public class ImportCSV : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ReadCSV();
    }

    void ReadCSV()
    {
        string path = (Application.dataPath + "/Other/testFile.csv");
        if( !(System.IO.File.Exists(path)) )
        {
            Debug.LogError("File could not be found.");
            return;
        }
        string csvFile = System.IO.File.ReadAllText(path); //reads all the text in the file
        string[] data = csvFile.Split(',','\n'); //split strings by commas and newlines

        foreach(string d in data)
        {
            Debug.Log(d);
        }

    }

}
