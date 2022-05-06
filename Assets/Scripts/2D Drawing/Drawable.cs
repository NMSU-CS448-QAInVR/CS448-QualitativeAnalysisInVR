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
 * When a data coding session is stored, the texture of the boards and notecards are stored as pngs files. 

    Initially, notecard will share the same texture object with the prefab object (prefab in the scene) in order to optimize performance and memory. A notecard will only be assigned a new texture when it is modified (either with drawing or png update).
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

    // at the start, we store as static variables, the initial colors of the boards and cards, so we don't need to recreate the original colros each time, to boost performance. 
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

    /*
        Give this object a new texture.
    */
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

    /*
        Update the texture of the drawing with an array of bytes of an image.
        Input:
        + data: the bytes to update hte texture with.
    */
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

    /*
        Get the current texture of the drawing as an array of bytes.
        Output: An array of bytes representing a PNG file. 
    */
    public byte[] GetTextureColor() {
        byte[] result = new byte[0];
        result = texture.EncodeToPNG();
        return result;
    } // end GetTexture

    /*
        Set pixels of the texture with colors. A new texture will be assigned if this texture has not been modified. 

        Input: See Texture2D.SetPixels() for more information about these inputs. 
        + x: 
        + y:
        + blockWidth
        + blockHeight
        + colors: The colors to set the pixels with.
    */
    public void SetPixels(int x, int y, int blockWidth, int blockHeight, Color[] colors) {
        if (!isModified()) {
            AssignNewTexture();
        } // end if

        texture.SetPixels(x, y, blockWidth, blockHeight, colors);
        SetModified();
    } // end SetPixels

    /*
        Set that the object is modified
    */
    public void SetModified() {
        modified = true;
    } // end SetModified

    /*
        Check if the object is modified.
        Output: True if the object is modified, else false. 
    */
    public bool isModified() {
        return modified;
    } // end isModified
}
