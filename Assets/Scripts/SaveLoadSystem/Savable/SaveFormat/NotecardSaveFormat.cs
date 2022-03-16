using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NotecardSaveFormat : SaveFormat
{
    // location
    private float x;
    private float y;
    private float z;

    // rotation
    private float quaternion_x;
    private float quaternion_y;
    private float quaternion_z;
    

    // color
    private float color_r;
    private float color_g;
    private float color_b;
    private float color_a;

    // text
    private string text;

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
        
        // angle
        quaternion_x = transform.rotation.x;
        quaternion_y = transform.rotation.y;
        quaternion_z = transform.rotation.z;

        // color
        Renderer renderer = note.GetComponent<Renderer>();
        if (renderer == null) {
            throw new Exception("Cannot find Renderer component in the game object");
        } // end if
        Color color = renderer.material.GetColor("_Color");
        if (color == null) {
            throw new Exception("Cannot find color component in material of renderer");
        } // end if
        Vector4 myColor = color;
        color_r = myColor.x;
        color_g = myColor.y;
        color_b = myColor.z;
        color_a = myColor.w;

        // text
    } // end NotecardSaveFormat

    public override void LoadObjectInto(GameObject notecard) {
        notecard.transform.position = new Vector3(x, y, z);
        notecard.transform.rotation = new Quaternion(quaternion_x, quaternion_y, quaternion_z, 0);

        Renderer renderer = notecard.GetComponent<Renderer>();
        if (renderer == null) {
            throw new Exception("Cannot find Renderer component in the game object");
        } // end if
        renderer.material.SetColor("_Color", new Color(color_r, color_g, color_b, color_a));
    } // end LoadObject
}
