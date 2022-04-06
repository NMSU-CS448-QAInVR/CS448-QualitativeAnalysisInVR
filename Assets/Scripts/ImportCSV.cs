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
        string[] data = csvFile.Split(','); //split strings by commas and newlines


        //this.textEdit.SetText("Lets see how long this text can go before getting cut off it can fit a lot more with the text size being reduced by half");
        float i = 0;
        Debug.Log("Generate Cards from CSV");
        foreach (string d in data)
        {
            Instantiate(notecard, new Vector3(0, 1, i), Quaternion.Euler(0, 0, 0));
            notecard.GetComponentInChildren<TextMeshPro>().SetText(d);
            i = i + (float)0.01;
            Debug.Log(d);
        }

    }

    public void Clicked()
    {
        ReadCSV();
    }

}
