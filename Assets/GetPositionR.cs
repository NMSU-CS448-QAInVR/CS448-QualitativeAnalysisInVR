using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPositionR : MonoBehaviour
{

     public static float RX;
     public static float RY;
     public static float RZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RX = transform.position.x;
        RY = transform.position.y;
        RZ = transform.position.z;
    }
}
