using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObjectOnCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision) {
        Object.Destroy(collision.gameObject);
    } // end OnCollisionEnter
}
