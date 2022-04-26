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

    public async Task UpdateTexture(LocationDrawn[] locations) {
        var data = texture.GetPixelData<Color>(0);
        await Task.Run(() => {
            for (int i = 0; i < locations.Length; ++i) {
                data[locations[i].x] = Color.red;
            } // end for i
        });

        texture.Apply();
       
    } // end UpdateTexture

    public async Task<List<LocationDrawn>> GetTextureColor() {
        List<LocationDrawn> result = new List<LocationDrawn>();
        var data = texture.GetPixelData<Color>(0);
        await Task.Run(() => {
            for (int i = 0; i < data.Length; ++i) {
                    Color color = data[i];
                    if (color == Color.white) {
                        continue;
                    } // end if
                    
                    LocationDrawn ld = new LocationDrawn();
                    ld.x = i;
                    result.Add(ld);
            } // end for i
        });
        return result;
    } // end GetTexture


}
