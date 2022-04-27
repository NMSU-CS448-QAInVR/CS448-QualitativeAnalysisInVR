using System.Collections;
using System.IO;
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
    public string title;

    // texture for drawings. 
    public string texture_file_name;
    public string saved_folder;

    private static int notecard_no = 0;

    public NotecardSaveFormat() : base(FormatType.NOTECARD) {
    } // end NotecardSaveFormat

    public override async Task UpdateData(GameObject note, string save_des_folder) {
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
        RelativeDisplay nte = note.GetComponent<RelativeDisplay>();
        text = nte.LongInfo;
        title = nte.Title;

        // texture for drawing
        Drawable dr = note.GetComponent<Drawable>();
        if (dr.isModified()) {
            byte[] texture_data = await dr.GetTextureColor();
            texture_file_name = "notecard" + (notecard_no++) + ".png";
            saved_folder = save_des_folder;
            string path = Path.Combine(saved_folder, texture_file_name);
            await FileManager.WriteBytesToAsync(path, texture_data);
        } else {
            texture_file_name = "";
        } // end else
      
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
        RelativeDisplay nte = notecard.GetComponent<RelativeDisplay>();
        if (nte != null) {
            nte.Title = title;  
            nte.LongInfo = text;
        } else {
            Debug.LogError("NotecardTextEdit is null");
        } // end else

        // // load texture
        Drawable dr = notecard.GetComponent<Drawable>();
        if (texture_file_name != "") {
            Debug.Log("I'm here in loading texture");
            string path = Path.Combine(saved_folder, texture_file_name);
            byte[] data = await FileManager.ReadBytesFromAsync(path);
            await dr.UpdateTexture(data);
            dr.SetModified();
        } //end if
    
        return true;
    } // end LoadObject
}
