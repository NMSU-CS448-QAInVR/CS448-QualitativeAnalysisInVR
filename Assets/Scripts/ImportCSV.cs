using UnityEngine;
using System;
using TMPro;

public class ImportCSV : MonoBehaviour
{
    private TextMeshPro textEdit;//textmeshpro object
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

        //Edit the text component of the card using TextMeshPro
        textEdit = this.GetComponentInChildren<TextMeshPro>();
        if(textEdit == null)
        {
            Debug.LogError("Notecard Object does not have the text component");
        }

        this.textEdit.SetText("Lets see how long this text can go before getting cut off by the card It cuts off here");

        //foreach(string d in data)
        //{
        //   Debug.Log(d);
        //}

    }

}
