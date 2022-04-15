using UnityEngine;
using UnityEngine.Events;
using System;
using TMPro;

public class ImportCSVMod : MonoBehaviour
{
    private static int i = 0; //keeps track of what value is being read from csv
    private static string[] data; //holds parsed strings from csv
    private UnityAction<string> createCardWithTextFunc;


    // Start is called before the first frame update
    void Start()
    {
        //try to read in file and save data to string array 'data' before clicked
       
        //Debug.Log("Data length: " + data.Length);
    }

    public void Initialize(GameObject prefab, string text, UnityAction<string> createCardWithText) {
        data = text.Split(','); //split string by commas
        createCardWithTextFunc = createCardWithText;
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
            string cardText = data[i].Replace("\n", "").Replace("\r", "");//remove newline and return from text
            Debug.Log("Generate card number " + i + " from CSV");
            Debug.Log(cardText);
            // create card
            createCardWithTextFunc(cardText);
            i++;
        }
    }

}
