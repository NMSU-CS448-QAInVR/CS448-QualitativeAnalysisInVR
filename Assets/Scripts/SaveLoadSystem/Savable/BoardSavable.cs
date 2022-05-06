using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;

/*
    Check Savable to learn more.
*/
public class BoardSavable : Savable
{
    public int id;
    public override async Task<SaveFormat> SaveObject(string save_des_folder) {
        // check if the components are here
        try {
            SaveFormat result = new BoardSaveFormat();
            await result.UpdateData(this.gameObject, save_des_folder);
            return result;
        } catch (Exception b) {
            Debug.Log("Exception here");
            if (b != null)
                Debug.LogError(b.ToString());
        }
        return null;
    } // end SaveObject

    public override void DeleteSelf() {
        Drawable dr = GetComponent<Drawable>();
        if (dr != null) {
            dr.ClearDrawing();
        } // end if
    } // end Delete
}
