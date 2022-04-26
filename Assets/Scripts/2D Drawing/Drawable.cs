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

    public async Task UpdateTexture(byte[] data) {
        texture.LoadImage(data);       
    } // end UpdateTexture

    public async Task<byte[]> GetTextureColor() {
        byte[] result = new byte[0];
        await Task.Run(() => {
            result = texture.EncodeToPNG();
        });
        return result;
    } // end GetTexture


}
