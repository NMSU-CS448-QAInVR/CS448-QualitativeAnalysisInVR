using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Savable : MonoBehaviour
{
    public virtual async Task<SaveFormat> SaveObject() {
        await Task.Delay(0);
        return null;
    }
}
