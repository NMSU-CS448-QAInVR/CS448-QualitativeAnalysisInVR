using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotecardTextEdit : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshPro textEditor;
    void Start()
    {
        textEditor = this.GetComponentInChildren<TextMeshPro>();
        // check null
        if (textEditor == null)
            Debug.LogError("Notecard Object does not have the text component");
    }

    // Update is called once per frame
    void Update()
    {    
    }

    void ChangeText(string text) {
        this.textEditor.SetText(text);
    }
}
