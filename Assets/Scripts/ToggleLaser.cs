/*
 * ToggleLaser.cs
 * Added by Fidel Soto on 3/7/22
 * Last Edited: []
 * Script will take care of toggling the laser.
 * When the user has their finger on the trigger sensor AND there is no object being interacted with, the laser will turn off.
 * Otherwise, the laser will stay on. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleLaser : MonoBehaviour
{

    public InputActionReference toggleLaserActionReference = null;


    private XRRayInteractor xrRayInteractor = null;
    private bool isGrabbing;


    // Start is called before the first frame update
    void Awake()
    {
        isGrabbing = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLaserState(toggleLaserActionReference.action.ReadValue<float>());
    }

    private void OnEnable()
    {
        xrRayInteractor = GetComponent<XRRayInteractor>();
        xrRayInteractor.selectEntered.AddListener(selectEntered);
        xrRayInteractor.selectExited.AddListener(selectExited);
    }

    private void OnDisable()
    {
        xrRayInteractor.selectEntered.RemoveListener(selectEntered);
        xrRayInteractor.selectExited.RemoveListener(selectExited);
    }

    private void UpdateIsGrabbing(bool grabbing)
    {
        isGrabbing = grabbing;
    }

    protected virtual void selectEntered(SelectEnterEventArgs args) => UpdateIsGrabbing(true);
    protected virtual void selectExited(SelectExitEventArgs args) => UpdateIsGrabbing(false);

    void UpdateLaserState(float value)
    {
        if (value != 0 && !isGrabbing)
        {
            xrRayInteractor.enabled = false;
        }
        else
        {
            xrRayInteractor.enabled = true;
        }
    }
}