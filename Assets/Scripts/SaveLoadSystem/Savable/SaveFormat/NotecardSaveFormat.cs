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
    public int[] drawn_location_x;

    public NotecardSaveFormat() : base(FormatType.NOTECARD) {
    } // end NotecardSaveFormat

    public override async Task UpdateData(GameObject note) {
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
        List<LocationDrawn> drawn_locations = await dr.GetTextureColor();
        drawn_location_x = new int[drawn_locations.Count];
        await Task.Run(() => {
            for (int i = 0; i < drawn_locations.Count; ++i) {
                drawn_location_x[i] = drawn_locations[i].x;
            } // end for i
        });
    } // end UpdateData

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
        Drawable dr = notecard.GetComponent<Drawable>();
        LocationDrawn[] locationDrawns = new LocationDrawn[drawn_location_x.Length];
        Debug.Log("Having locations: " + locationDrawns.Length);
        await Task.Run(() => {
            for (int i = 0; i < locationDrawns.Length; ++i) {
                locationDrawns[i].x = drawn_location_x[i];
            } // end for i
        });
        
        await dr.UpdateTexture(locationDrawns);

        return true;
    } // end LoadObject
}
