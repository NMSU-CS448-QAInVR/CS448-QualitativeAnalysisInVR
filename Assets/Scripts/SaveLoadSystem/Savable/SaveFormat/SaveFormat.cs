using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FormatType {
    NOTECARD, BOARD
}

public abstract class SaveFormat
{
    private FormatType type;

    // Call this function to set the type
    public SaveFormat(FormatType myType) {
        type = myType;
    } // end SaveFormat

    public FormatType getType() {
        return type;
    } // end getType

    public abstract void LoadObjectInto(GameObject obj);
}
