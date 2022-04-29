using UnityEngine;
using UnityEngine.Events;
using System;
using System.Text.RegularExpressions;
using TMPro;

public class ImportCSVMod : MonoBehaviour
{
    private int i = 0; //keeps track of what value is being read from csv
    private string[] data; //holds parsed strings from csv
    private Func<string, string, GameObject> createCardWithTextFunc;
    private string title;

    private GameObject CardLocation;


    // Start is called before the first frame update
    void Start()
    {
        //try to read in file and save data to string array 'data' before clicked
       
        //Debug.Log("Data length: " + data.Length);
        CardLocation = GetComponentInChildren<ImportCardLocationScript>().gameObject;
        
    }

    public void Initialize(GameObject prefab, string text, Func<string, string, GameObject> createCardWithText) {
        data = text.Split(new string[] {"\n", "\r", "\r\n"}, StringSplitOptions.None); //split string by commas
        //Debug.Log("I'm here");
        //Debug.Log("There are " + data.Length + " lines");
        createCardWithTextFunc = createCardWithText;
        //title = data[0].Replace("\n", "").Replace("\r", "");//remove newline and return from text
    } // end Initialize

    public void Clicked()
    {
        //check to see if all values have been printed
        if (i >= data.Length)
        {
            Debug.Log("All cards printed do nothing");
        }
        else
        {
            string[] values = Regex.Split(data[i], "[,]{1}(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            if (values.Length < 3) {
                Debug.Log("Less thean the required format");
                return;
            }
            string title = "Participant: " + values[0] + "\n" +
                            "Quote Number: " + values[1];
            //string cardText = data[i].Replace("\n", "").Replace("\r", "");//remove newline and return from text
            string cardText = values[2];
            Debug.Log("Generate card number " + i + " from CSV");
            Debug.Log(cardText);
            // create card
            GameObject gObj = createCardWithTextFunc(title, cardText);
            gObj.transform.position = CardLocation.transform.position + new Vector3(0, gObj.transform.localScale.y / 2f, 0);
            gObj.transform.rotation = this.transform.rotation;
            i++;
        }
    }

}
