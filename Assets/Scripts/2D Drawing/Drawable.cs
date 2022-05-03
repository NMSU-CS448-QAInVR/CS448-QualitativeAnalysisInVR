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
       
        
        Texture2D myTexture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        Renderer myRenderer = GetComponent<Renderer>();
        ImageConversion.LoadImage(myTexture, data);  
        myRenderer.material.mainTexture = myTexture;
        texture = myTexture;
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
