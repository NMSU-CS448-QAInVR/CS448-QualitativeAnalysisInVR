using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Savable : MonoBehaviour
{
    public virtual async Task<SaveFormat> SaveObject(string save_des_folder) {
        await Task.Delay(0);
        return null;
    }
}
