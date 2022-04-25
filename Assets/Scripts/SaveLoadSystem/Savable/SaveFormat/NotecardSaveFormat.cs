using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

[Serializable]
public class NotecardSaveFormat : SaveFormat
{
    // location
    public float x;
    
    public float y;
    
    public float z;

    // rotation
    
    public float quaternion_x;
    
    public float quaternion_y;
    
    public float quaternion_z;
    public float quaternion_w;

    // scale
    public float x_scale;
    public float y_scale;
    public float z_scale;
    

    // color
    
    public float color_r;
    
    public float color_g;
    
    public float color_b;
    
    public float color_a;

    // text
    
    public string text;
    public float font_size;

    // texture for drawings. 
    public float[] texture_color_r;
    public float[] texture_color_g;
    public float[] texture_color_b;
    public float[] texture_color_a;

    public NotecardSaveFormat() : base(FormatType.NOTECARD) {
    } // end NotecardSaveFormat

    public NotecardSaveFormat(GameObject note) : base(FormatType.NOTECARD) {
        Transform transform = note.transform;
        if (transform == null) {
            throw new Exception("Cannot find transform component in the game object");
        } // end if
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;

        x_scale = transform.localScale.x;
        y_scale = transform.localScale.y;
        z_scale = transform.localScale.z;
        
        // angle
        quaternion_x = transform.rotation.x;
        quaternion_y = transform.rotation.y;
        quaternion_z = transform.rotation.z;
        quaternion_w  = transform.rotation.w;

        // color
        Apperance apperance = note.GetComponent<Apperance>();
        Vector4 myColor = apperance.GetColor();
        color_r = myColor.x;
        color_g = myColor.y;
        color_b = myColor.z;
        color_a = myColor.w;

        // text
        NotecardTextEdit nte = note.GetComponent<NotecardTextEdit>();
        text = nte.GetText();
        font_size = nte.GetTextFontSize();

        // texture for drawing
        Drawable dr = note.GetComponent<Drawable>();
        Color[] color = dr.GetTextureColor();
        // texture_color_r = new float[color.Length];
        // texture_color_g = new float[color.Length];
        // texture_color_b = new float[color.Length];
        // texture_color_a = new float[color.Length];
        // for (int i = 0; i < color.Length; ++i) {
        //     texture_color_r[i] = color[i].r;
        //     texture_color_g[i] = color[i].g;
        //     texture_color_b[i] = color[i].b;
        //     texture_color_a[i] = color[i].a;
        // } // end for i
    } // end NotecardSaveFormat

    public override async Task<bool> LoadObjectInto(GameObject notecard) {
        // set position and rotation
        notecard.transform.position = new Vector3(x, y, z);
        notecard.transform.rotation = new Quaternion(quaternion_x, quaternion_y, quaternion_z, quaternion_w);
        notecard.transform.localScale = new Vector3(x_scale, y_scale, z_scale);

        Apperance apperance = notecard.GetComponent<Apperance>();
        if (apperance == null)
            Debug.LogError("Apperance is null");
        // set color
        Color newColor = new Color(color_r, color_g, color_b, color_a);
        if (newColor == null)
            Debug.LogError("New color is null");
        apperance.ChangeColor(newColor);

        // set text and font size
        NotecardTextEdit nte = notecard.GetComponent<NotecardTextEdit>();
        if (nte == null)
            Debug.LogError("NotecardTextEdit is null");
        //nte.SetTextFontSize(font_size);
        nte.ChangeText(text);

        // load texture
        // if (texture_color_a != null) {
        //     Drawable dr = notecard.GetComponent<Drawable>();
        //     Color[] color = new Color[texture_color_a.Length];
        //     for (int i = 0; i < color.Length; ++i) {
        //         color[i] = new Color(texture_color_r[i], texture_color_b[i], texture_color_g[i], texture_color_r[i]);
        //     } // end for i
        //     dr.UpdateTexture(color);
        // } //end if

        return true;
    } // end LoadObject
}
