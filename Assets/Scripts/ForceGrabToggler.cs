using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ForceGrabToggler : MonoBehaviour
{

    public InputActionReference enableForceGrab;

    private bool isGrabbing;
    private bool isForceGrabEnabled;

    XRRayInteractor xrRayInteractor = null;

    private void OnEnable()
    {
        xrRayInteractor = GetComponent<XRRayInteractor>();
        xrRayInteractor.selectEntered.AddListener(SelectEntered);
        xrRayInteractor.selectExited.AddListener(SelectExited);
    }

    private void OnDisable()
    {
        xrRayInteractor.selectEntered.RemoveListener(SelectEntered);
        xrRayInteractor.selectExited.RemoveListener(SelectExited);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateForceGrabState(enableForceGrab.action.ReadValue<float>());
        UpdateLaserRenderer();
    }

    protected virtual void SelectEntered(SelectEnterEventArgs args) => UpdateIsGrabbing(true);
    protected virtual void SelectExited(SelectExitEventArgs args) => UpdateIsGrabbing(false);

    private void UpdateForceGrabState(float val)
    {
        if (val > 0 && !isGrabbing)
        {
            xrRayInteractor.useForceGrab = true;
            xrRayInteractor.allowAnchorControl = false;
        }
        else if (val <= 0 && !isGrabbing)
        {
            xrRayInteractor.useForceGrab = false;
            xrRayInteractor.allowAnchorControl = true;
        }
    }

    private void UpdateIsGrabbing(bool grabbing)
    {
        isGrabbing = grabbing;
    }

    private void UpdateLaserRenderer()
    {
        if (isGrabbing && xrRayInteractor.useForceGrab)
        {
            XRInteractorLineVisual xrInteractorLineVisual = GetComponent<XRInteractorLineVisual>();
            xrInteractorLineVisual.enabled = false;
        }
        else
        {
            XRInteractorLineVisual xrInteractorLineVisual = GetComponent<XRInteractorLineVisual>();
            xrInteractorLineVisual.enabled = true;
        }
    }

}
