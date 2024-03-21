using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private string doorOpen = "DoorOpen";
    [Tooltip("This string is the name of the animation state. Only change if animator state name has changed")] 
    [SerializeField] private string doorClose = "DoorClose";
    [Tooltip("This string is the name of the animation state. Only change if animator state name has changed")]
    [SerializeField] private bool ChangeDoorState = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
        if(ChangeDoorState)
        {
            ChangeDoorState = false;
            TriggerAnimation();
        }
    }
    public void TriggerAnimation()
    {
        if (doorAnimator.GetCurrentAnimatorStateInfo(0).IsName(doorOpen))
        {
            doorAnimator.Play(doorClose);
        }
        else
        {
            doorAnimator.Play(doorOpen);
        }

        
    }
}
