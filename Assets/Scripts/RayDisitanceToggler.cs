using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RayDisitanceToggler : MonoBehaviour
{
    public XRRayInteractor hand;
    public XRInteractorLineVisual lineVisual;

    private void Start()
    {
        hand = GetComponent<XRRayInteractor>();
        lineVisual = GetComponent<XRInteractorLineVisual>();
    }

    void Update()
    {
        if (hand.selectTarget?.tag == "Pen" && Vector3.Distance(transform.position, hand.selectTarget.gameObject.transform.position) < 0.35f)
        {
            lineVisual.enabled = false;
        }
        else
        {
            lineVisual.enabled = true;
        }
    }
}
