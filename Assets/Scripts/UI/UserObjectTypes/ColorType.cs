using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    An object attached to a menu component to define the type of color that object has. 
    Precondition: The color is from an Image object.
    Image is a built-in class of Unity. 

    This is currently being used by the create card menu in the card spawning buttons to spawn cards with multiple colors. This will help us know which color to spawn the card from.
*/
public class ColorType : MonoBehaviour
{
    private Image myImage;
    void Awake() {
        myImage = this.gameObject.GetComponent<Image>();
        if (myImage == null)
            Debug.LogError("Cannot find image for color type");
    }

    /*
        Get the color of this menu component.
    */
    public Color GetValue() {
        return myImage.color;
    } // end Getvalue
}
