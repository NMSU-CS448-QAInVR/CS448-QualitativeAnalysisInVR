using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/*
    The base class that can be assigned to each GameObject so that we get the SaveFormat from that object.
*/
public class Savable : MonoBehaviour
{   
    void Awake() {
    }

    void Update() {}

    /*
        Get the SaveFormat describing the GameObject that this object is assigned to. 
    */
    public virtual async Task<SaveFormat> SaveObject(string save_des_folder) {
        await Task.Delay(0);
        return null;
    }

    /*
        Destroy the GameObject this object is assigned to.
    */
    public virtual void DeleteSelf() {
        Destroy(this.gameObject, 1);
    } // end Delete
}
