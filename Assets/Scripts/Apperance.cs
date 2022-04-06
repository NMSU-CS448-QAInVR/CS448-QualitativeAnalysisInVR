using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apperance : MonoBehaviour
{
    private Material myMaterial;

    // Start is called before the first frame update
    void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null) {
            Debug.LogError("No Renderer found");
            return;
        } // end if

        this.myMaterial = renderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeColor(Color newColor) {
        this.myMaterial.SetColor("_Color", newColor);
    } // end ChangeColor

    public Color GetColor() {
        return this.myMaterial.GetColor("_Color");
    } // end GetColor

    void Resize(float scale) {
        this.transform.localScale *= scale;
    } // end Resize
}
