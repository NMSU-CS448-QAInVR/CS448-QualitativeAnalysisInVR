using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorType : MonoBehaviour
{
    private Renderer myRenderer;
    void Awake() {
        myRenderer = this.GetComponent<Renderer>();
        if (myRenderer)
            Debug.LogError("Cannot find renderer for color type");
    }

    public Color GetValue() {
        return myRenderer.material.GetColor("_Color");
    } // end Getvalue
}
