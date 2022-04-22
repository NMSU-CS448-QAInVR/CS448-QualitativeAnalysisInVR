using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDeleteController3D : MonoBehaviour
{

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
        if (collision.gameObject.tag == "LineCollider" && 
            parentDrawController.isGrabbed && 
            parentDrawController.isErasing)
        {
            Object.Destroy(collision.gameObject.transform.parent.gameObject);
        }
    }
}
