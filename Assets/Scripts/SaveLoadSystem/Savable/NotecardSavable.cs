using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;


public class NotecardSavable : Savable
{
    public override async Task<SaveFormat> SaveObject(string save_des_folder) {
        // check if the components are here
        try {
            SaveFormat result = new NotecardSaveFormat();
            await result.UpdateData(this.gameObject, save_des_folder);
            return result;
        } catch (Exception b) {
            Debug.Log("Exception here");
            if (b != null)
                Debug.LogError(b.ToString());
        }
        return null;
    } // end SaveObject
}
