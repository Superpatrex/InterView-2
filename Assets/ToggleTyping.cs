using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTyping : MonoBehaviour
{
    public GameObject ComputerCamera;
    public GameObject InterviewElements;
    public GameObject InterviewTyping;
    public GameObject InterviewNormal;
    public GameObject TypingConsole;
    public GameObject InterviewerHead;
    public GameObject InterviewerHands;

    public bool toggleTyping = true;
    private bool typing = false;
    private void Update()
    {
        if(toggleTyping)
        {
            Toggle();
            toggleTyping = false;
        }
    }
    public void Toggle()
    {
        if(typing)
        {
            TypingConsole.SetActive(false);
            ComputerCamera.SetActive(false);
            // Briefly disable objects to disable lazy follow
            InterviewerHead.SetActive(false);
            InterviewerHands.SetActive(false);

            InterviewElements.transform.position = InterviewNormal.transform.position;
            InterviewElements.transform.rotation = InterviewNormal.transform.rotation;

            InterviewerHead.SetActive(true);
            InterviewerHands.SetActive(true);
        }
        else
        {
            TypingConsole.SetActive(true);
            ComputerCamera.SetActive(true);

            // Briefly disable objects to disable lazy follow
            InterviewerHead.SetActive(false);
            InterviewerHands.SetActive(false);

            InterviewElements.transform.position = InterviewTyping.transform.position;
            InterviewElements.transform.rotation = InterviewTyping.transform.rotation;

            InterviewerHead.SetActive(true);
            InterviewerHands.SetActive(true);
        }
        typing = !typing;
    }
}
