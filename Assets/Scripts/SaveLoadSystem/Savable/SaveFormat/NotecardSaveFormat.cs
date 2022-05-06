using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

/*
    Check SaveFormat for more info.

    The savings of 2D drawings of a notecard will be stored externally as a PNG file. The name of the file is texture_file_name, the folder where the file is saved_folder. The saved_folder is currently the name of the folder of the session where this will be stored. 
    The name will be "notecard" + index i from the static variable notecard_no. notecard_no will increment after each notecard returns a NotecardSaveFormat. 
    This notecard_no should be reset in every load using the function ResetNotecardNo. 
*/
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
            byte[] texture_data = dr.GetTextureColor();
            texture_file_name = "notecard" + (notecard_no++) + ".png";
            saved_folder = save_des_folder;
            string path = Path.Combine(saved_folder, texture_file_name);
            await FileManager.WriteBytesToAsync(path, texture_data);
        } else {
            texture_file_name = "";
        } // end else
      
    } // end UpdateData
    public static void ResetNotecardNo() {
        notecard_no = 0;
    } // end ResetNotecardNo
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
            string path = Path.Combine(saved_folder, texture_file_name);
            //Debug.Log("I'm here in loading texture: " + path);
            byte[] data = FileManager.ReadBytesFrom(path);
            if (data != null) {
                //Debug.Log("Load texture here");
                dr.UpdateTexture(data);
                dr.SetModified();
            }
        } //end if
    
        return true;
    } // end LoadObject
}
