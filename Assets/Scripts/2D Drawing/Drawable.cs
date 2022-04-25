using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Drawable : MonoBehaviour
{

    public Texture2D texture;
    public int width, height;

    [HideInInspector]
    public Vector2 textureSize;

    private void Start()
    {
        textureSize = new Vector2(width, height);
        Renderer renderer = GetComponent<Renderer>();

        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        Color[] colors = Enumerable.Repeat(Color.white, width * height).ToArray();

        texture.SetPixels(0, 0, width, height, colors);
        texture.Apply(true);
        renderer.material.mainTexture = texture;
    }

    public void UpdateTexture(Color[] color) {
        texture.SetPixels(0, 0, width, height, color);
    } // end UpdateTexture

    public Color[] GetTextureColor() {
        return texture.GetPixels();
    } // end GetTexture


}
