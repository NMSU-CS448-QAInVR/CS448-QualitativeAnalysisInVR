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

    private void Awake()
    {
        textureSize = new Vector2(width, height);
        Renderer myRenderer = GetComponent<Renderer>();

        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        Color[] colors = Enumerable.Repeat(Color.white, width * height).ToArray();

        texture.SetPixels(0, 0, width, height, colors);
        texture.Apply(true);
        myRenderer.material.mainTexture = texture;
    }

    public void UpdateTexture(byte[] data) {
        Texture2D myTexture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        Renderer myRenderer = GetComponent<Renderer>();
        ImageConversion.LoadImage(myTexture, data);  
        myRenderer.material.mainTexture = myTexture;
        texture = myTexture;
    } // end UpdateTexture

    public async Task<byte[]> GetTextureColor() {
        byte[] result = new byte[0];
        result = texture.EncodeToPNG();
        return result;
    } // end GetTexture

    public void SetPixels(int x, int y, int blockWidth, int blockHeight, Color[] colors) {
        texture.SetPixels(x, y, blockWidth, blockHeight, colors);
        modified = true;
    } // end SetPixels

    public void SetModified() {
        modified = true;
    } // end SetModified

    public bool isModified() {
        return modified;
    } // end isModified


}
