/*
 * Drawable.cs
 * 
 * Written by Fidel Soto and Long Tran
 * Implementation borrowed from and extended upon https://www.youtube.com/watch?v=sHE5ubsP-E8
 * 
 * This script takes care of holding the texture of the board or notecard that it is attached to.
 * There used to be issues with color and shading looking off compared to the boards and notecards. 
 * So when the script starts, a preemptive color is applied to the board so that it matches with the 
 * colors applied by the pen better. 
 * 
 * When a data coding session is stored, the texture of the boards and notecards are stored as pngs. 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

public class Drawable : MonoBehaviour
{

    public Texture2D texture;
    public int width, height;

    [HideInInspector]
    public Vector2 textureSize;

    private bool modified = false;

    // blank colors. 
    private static Color[] notecards_colors = null;
    private static Color[] board_colors = null;

    private void Awake()
    {
        // Only apply textures to boards when this script awakes. 
        // This is to save on the performance cost of having to create
        // new textures every time we create a card. 
        if (!transform.name.Contains("Clone"))
        {
            int layer = transform.gameObject.layer;
            if (layer == LayerMask.NameToLayer("NoteCard")) {
                Drawable.notecards_colors = Enumerable.Repeat(Color.white, width * height).ToArray();
            } else {
                Drawable.board_colors = Enumerable.Repeat(Color.white, width * height).ToArray();
            } // end else
            
            AssignNewTexture();
        }
    }

    // Clear drawing when we load a new session.
    // This is called in the save load system
    public void ClearDrawing() {
        Color[] colors = null;
        int layer = transform.gameObject.layer;
        if (layer == LayerMask.NameToLayer("NoteCard")) {
            colors = Drawable.notecards_colors;
        } else {
            colors = Drawable.board_colors;
        } // end else

        texture.SetPixels(0, 0, width, height, colors);
        texture.Apply(true);
    } // end ClearDrawing

    // Assign the new texture here
    public void AssignNewTexture() {
        Color[] colors = null;
        int layer = transform.gameObject.layer;
        if (layer == LayerMask.NameToLayer("NoteCard")) {
            colors = Drawable.notecards_colors;
        } else {
            colors = Drawable.board_colors;
        } // end else

        textureSize = new Vector2(width, height);
        Renderer myRenderer = GetComponent<Renderer>();

        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        texture.SetPixels(0, 0, width, height, colors);
        texture.Apply(true);
        myRenderer.material.mainTexture = texture;
    } // end AssignNewTexture


    public void UpdateTexture(byte[] data) {
        int layer = transform.gameObject.layer;
        if (layer == LayerMask.NameToLayer("NoteCard")) {
            if (!isModified()) {
                AssignNewTexture();
            } // end if
        } // end if 
       
        ImageConversion.LoadImage(texture, data);  
        SetModified();
    } // end UpdateTexture

    public byte[] GetTextureColor() {
        byte[] result = new byte[0];
        result = texture.EncodeToPNG();
        return result;
    } // end GetTexture

    public void SetPixels(int x, int y, int blockWidth, int blockHeight, Color[] colors) {
        if (!isModified()) {
            AssignNewTexture();
        } // end if

        texture.SetPixels(x, y, blockWidth, blockHeight, colors);
        SetModified();
    } // end SetPixels

    public void SetModified() {
        modified = true;
    } // end SetModified

    public bool isModified() {
        return modified;
    } // end isModified


}
