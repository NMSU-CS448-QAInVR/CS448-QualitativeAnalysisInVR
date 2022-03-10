using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorType : MonoBehaviour
{
    private Image myImage;
    void Awake() {
        myImage = this.gameObject.GetComponent<Image>();
        if (myImage == null)
            Debug.LogError("Cannot find image for color type");
    }

    public Color GetValue() {
        return myImage.color;
    } // end Getvalue
}
