using System.Collections.Generic;
using System.Xml.Serialization;

/*
    An object containing a list of SaveFormat to save a list of format.
*/
public class ListSaveFormat {
    public List<SaveFormat> list_format;

    public ListSaveFormat() {
        list_format = new List<SaveFormat>();
    } // end ListSaveFormat

}