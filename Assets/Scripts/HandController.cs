using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class HandController : MonoBehaviour
{
    ActionBasedController controller;
    public Hand hand;

    // Start is called before the first frame update
    void Start()
    {
        //set up
        controller = GetComponent<ActionBasedController>();
    }

    // Update is called once per frame
    void Update()
    {
        //call setGrip and setTrigger and pass float values from buttons
        hand.SetGrip(controller.selectAction.action.ReadValue<float>());
        hand.SetTrigger(controller.activateAction.action.ReadValue<float>());
    }
}
