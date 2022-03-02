using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apperance : MonoBehaviour
{
    private Material myMaterial;

    // Start is called before the first frame update
    void Start()
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

    void ChangeColor(Color newColor) {
        this.myMaterial.color = newColor;
    } // end ChangeColor

    void Resize(float scale) {
        this.transform.localScale *= scale;
    } // end Resize
}
