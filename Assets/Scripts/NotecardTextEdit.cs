using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotecardTextEdit : MonoBehaviour
{
    // Start is called before the first frame update
    private TMP_InputField textEditor;
    void Awake()
    {
        textEditor = this.GetComponentInChildren<TMP_InputField>();
        // check null
        if (textEditor == null)
            Debug.LogError("Notecard Object does not have the text component");
    }

    // Update is called once per frame
    void Update()
    {    
    }

    public void ChangeText(string text) {
        this.textEditor.SetTextWithoutNotify(text);
    } // change text

    public string GetText() {
        return this.textEditor.text;
    } // end GetText

    public void SetTextFontSize(float fontSize) {
        this.textEditor.pointSize = fontSize;
    } // end SetTextFontSize

    public float GetTextFontSize() {
        return this.textEditor.pointSize;
    } // end GetTextFontSize

    public void InvokeEdit() {
        this.textEditor.Select();
    } // InvokeEdit

    public void SetTextColor(Color newColor) {

    } // end SetTextColor

}
