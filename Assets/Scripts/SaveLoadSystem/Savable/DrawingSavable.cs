using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;


public class DrawingSavable : Savable
{
    public override async Task<SaveFormat> SaveObject() {
        // check if the components are here
        try {
            SaveFormat result = new DrawingSaveFormat();
            await result.UpdateData(this.gameObject);
            return result;
        } catch (Exception b) {
            Debug.Log("Exception here");
            if (b != null)
                Debug.LogError(b.ToString());
        }
        return null;
    } // end SaveObject
}
