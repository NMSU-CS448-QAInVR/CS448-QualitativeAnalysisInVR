using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CategoryTypeEnum {
    BOARD, ABSTRACT
}

[System.Serializable]
public class CategoryType : MonoBehaviour {
   
    [SerializeReference]
    CategoryTypeEnum type;

    public CategoryTypeEnum GetValue() {
        return type;
    } // end Getvalue
}
