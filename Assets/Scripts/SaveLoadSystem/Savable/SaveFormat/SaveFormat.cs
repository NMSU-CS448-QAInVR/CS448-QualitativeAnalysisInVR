using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using UnityEngine;

[XmlType("SaveFormat")]
[XmlInclude(typeof(NotecardSaveFormat))]
public class SaveFormat
{
    public FormatType type;

    // Call this function to set the type

    public SaveFormat() {
        type = FormatType.NONE;
    } // end SaveFormat

    public SaveFormat(FormatType myType) {
        type = myType;
    } // end SaveFormat

    public FormatType getType() {
        return type;
    } // end getType

    public virtual void  LoadObjectInto(GameObject obj) {
    }
}
