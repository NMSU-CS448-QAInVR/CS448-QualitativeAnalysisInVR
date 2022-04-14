using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPosition : MonoBehaviour
{
    
    public static float LX;
    public static float LY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       LX = transform.position.x;
       LY = transform.position.y;
    }

    
}
