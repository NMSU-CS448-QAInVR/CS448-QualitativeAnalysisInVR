using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Animator))]

public class Hand : MonoBehaviour
{
    public float speed;
    Animator animator;
    private float gripTarget;
    private float triggerTarget;
    private float currentGrip;
    private float triggerCurrent;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHand();
    }


    internal void SetGrip(float v){
        gripTarget = v;
    }

    internal void SetTrigger(float v){
        triggerTarget = v;
    }

    void AnimateHand(){
        if(gripTarget != currentGrip){
            currentGrip = Mathf.MoveTowards(currentGrip, gripTarget, Time.deltaTime * speed);
            animator.SetFloat("grip", currentGrip);
            Debug.Log(currentGrip);
        }
        if(triggerTarget != triggerCurrent){
            triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * speed);
            animator.SetFloat("trigger", triggerCurrent);
            Debug.Log(triggerCurrent);
        }
    }
}
