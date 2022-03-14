using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotecardSaveFormat : SaveFormat
{
    // location
    private float x;
    private float y;
    private float z;

    // rotation
    

    // color
    private float color_r;
    private float color_g;
    private float color_b;

    // text
    private string text;

    public NotecardSaveFormat() : base(FormatType.NOTECARD) {
    } // end NotecardSaveFormat

    public NotecardSaveFormat(GameObject note) : base(FormatType.NOTECARD) {
    } // end NotecardSaveFormat

    public override void LoadObjectInto(GameObject notecard) {
    } // end LoadObject
}
