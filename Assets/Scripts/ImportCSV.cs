using UnityEngine;
using System;
using TMPro;

public class ImportCSV : MonoBehaviour
{
    public GameObject notecard;
    private TextMeshPro textEdit;//textmeshpro object
    private static int i = 0; //keeps track of what value is being read from csv
    private static string[] data; //holds parsed strings from csv


    // Start is called before the first frame update
    void Start()
    {
        //try to read in file and save data to string array 'data' before clicked
        string path = (Application.dataPath + "/Other/testFile.csv");
        if (!(System.IO.File.Exists(path)))
        {
            Debug.LogError("File could not be found.");
            return;
        }
        string csvFile = System.IO.File.ReadAllText(path); //reads all the text in the file as one long string
        data = csvFile.Split(','); //split string by commas
        //Debug.Log("Data length: " + data.Length);
    }

    public void Clicked()
    {
        //check to see if all values have been printed
        if (i >= data.Length)
        {
            Debug.Log("All cards printed do nothing");
        }
        else
        {
            string cardText = data[i].Replace("\n", "").Replace("\r", "");//remove newline and return from text
            Debug.Log("Generate card number " + i + " from CSV");
            Debug.Log(cardText);
            Instantiate(notecard, new Vector3(0, 1, 0), Quaternion.Euler(0, 0, 0));//create note card object
            notecard.GetComponentInChildren<TextMeshPro>().SetText(cardText); //set text on child component, TextMeshPro, of Notecard object
            i++;
        }
    }

}
