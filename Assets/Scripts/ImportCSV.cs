using UnityEngine;
using System;
using TMPro;

public class ImportCSV : MonoBehaviour
{
    private TextMeshPro textEdit;//textmeshpro object
    public GameObject notecard;
    //public Object note;
    // Start is called before the first frame update

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
        //textEdit = notecard.GetComponentInChildren<TextMeshPro>();
        //if(textEdit == null)
        //{
        //    Debug.LogError("Notecard Object does not have the text component");
        //}

        //this.textEdit.SetText("Lets see how long this text can go before getting cut off it can fit a lot more with the text size being reduced by half");
        float i = 0;
        foreach (string d in data)
        {
            Instantiate(notecard, new Vector3(0, 1, i), Quaternion.Euler(0, 0, 0));
            notecard.GetComponentInChildren<TextMeshPro>().SetText(d);
            Debug.Log(d);
            i = i + (float)0.01;
        }

    }

    public void Clicked()
    {
        Debug.Log("Generate Cards from CSV");
        ReadCSV();
    }

}
