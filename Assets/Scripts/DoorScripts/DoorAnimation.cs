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
    [SerializeField] private bool IsLocked = false;
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
        if(!IsLocked)
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
    public void LockDoor()
    {
        if (doorAnimator.GetCurrentAnimatorStateInfo(0).IsName(doorOpen))
        {
            doorAnimator.Play(doorClose);
        }
        IsLocked = true;
    }

    public void UnlockDoor()
    {
        if (!doorAnimator.GetCurrentAnimatorStateInfo(0).IsName(doorOpen))
        {
            doorAnimator.Play(doorOpen);
        }
        IsLocked = false;
    }
}
