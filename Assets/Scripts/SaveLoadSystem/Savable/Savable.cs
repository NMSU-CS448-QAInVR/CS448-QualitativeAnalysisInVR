using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savable : MonoBehaviour
{
    public virtual SaveFormat SaveObject() {
        return null;
    }
}
