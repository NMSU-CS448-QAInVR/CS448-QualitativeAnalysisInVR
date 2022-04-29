using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Savable : MonoBehaviour
{   
    void Awake() {
    }

    void Update() {}
    public virtual async Task<SaveFormat> SaveObject(string save_des_folder) {
        await Task.Delay(0);
        return null;
    }

    public virtual void DeleteSelf() {
        Destroy(this.gameObject, 1);
    } // end Delete
}
