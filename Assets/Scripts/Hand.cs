using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
//Add to hand object to create animation on trigger and grip


[RequireComponent(typeof(Animator))]


public class Hand : MonoBehaviour
{
    public float speed;   //use to set animation speed in inspector
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
        AnimateHand(); //call animate hand
    }

    //set gripTarget to current button value
    internal void SetGrip(float v){
        gripTarget = v;
    }
    //set triggerTarget to current button value
    internal void SetTrigger(float v){
        triggerTarget = v;
    }

    void AnimateHand(){
        if(gripTarget != currentGrip){ //current and target are not same 
            currentGrip = Mathf.MoveTowards(currentGrip, gripTarget, Time.deltaTime * speed); //began moving animation
            animator.SetFloat("grip", currentGrip);
           // Debug.Log(currentGrip);
        }
        if(triggerTarget != triggerCurrent){
            triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * speed); //began moving animation
            animator.SetFloat("trigger", triggerCurrent);
           // Debug.Log(triggerCurrent);
        }
    }
}
