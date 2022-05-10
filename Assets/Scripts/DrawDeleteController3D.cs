/*
 * DrawDeleteController3D.cs
 * Written by Fidel Soto
 * 
 * This script takes care of deleting 3D drawings when the user holds the 
 * pen up to a 3D drawing while pressing A on the oculus controller.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDeleteController3D : MonoBehaviour
{

    // info from the parent pen is necessary to determine if the
    // pen is grabbed and if the user is pressing A
    public GameObject parentPen = null;
    private DrawController3D parentDrawController;


    // Start is called before the first frame update
    void Start()
    {
        parentDrawController = parentPen.GetComponent<DrawController3D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log($"parentDrawController.isGrabbed is {parentDrawController.isGrabbed}. " +
        //     $"parentDrawController.isErasing is {parentDrawController.isErasing}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log($"OnCollisionEnter Entered. parentDrawController.isGrabbed is {parentDrawController.isGrabbed}. " +
        //     $"parentDrawController.isErasing is {parentDrawController.isErasing}");
        
        // When a sphere collider of a 3D drawing is detected delte the draw parent
        if (collision.gameObject.tag == "LineCollider" && 
            parentDrawController.isGrabbed && 
            parentDrawController.isErasing)
        {
            Object.Destroy(collision.gameObject.transform.parent.gameObject);
        }
    }
}
