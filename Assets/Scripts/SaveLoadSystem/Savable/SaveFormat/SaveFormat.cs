using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Text;
using UnityEngine;

/*
    The object to describe an object to save.
*/
[XmlType("SaveFormat")]
[XmlInclude(typeof(NotecardSaveFormat))]
public class SaveFormat
{
    // the type of object this object is describing.
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

    /*
        Load the information of this object into the GameObject in the argument. 
        Input: 
        + obj: The GameObject to load into
    */
    public virtual async Task<bool>  LoadObjectInto(GameObject obj) {
        await Task.Delay(0);
        return true;
    }

    /*
        Update the information that this information with the information from the GameObject obj.
        Input:
        + obj: The GameObject to get the information from.
        + save_des_folder: The folder to store any external data (like PNG files for 2D drawings) to.
    */
    public virtual async Task UpdateData(GameObject obj, string save_des_folder) {
        await Task.Delay(0);
    } // end UpdateData
}
