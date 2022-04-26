using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Threading.Tasks;
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

    public virtual async Task<bool>  LoadObjectInto(GameObject obj) {
        await Task.Delay(0);
        return true;
    }

    public virtual async Task UpdateData(GameObject obj) {
        await Task.Delay(0);
    } // end UpdateData
}
