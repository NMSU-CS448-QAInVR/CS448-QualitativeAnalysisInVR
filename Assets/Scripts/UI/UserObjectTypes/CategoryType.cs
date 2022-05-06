using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CategoryTypeEnum {
    BOARD, ABSTRACT
}

/*
    An Object to define the type of Category (Board). This is currently not used.
*/
[System.Serializable]
public class CategoryType : MonoBehaviour {
   
    [SerializeReference]
    CategoryTypeEnum type;

    public CategoryTypeEnum GetValue() {
        return type;
    } // end Getvalue
}
