using OpenAI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypingManager : MonoBehaviour
{
    public GameObject ComputerCamera;
    public GameObject InterviewElements;
    public GameObject InterviewTyping;
    public GameObject InterviewNormal;
    public GameObject TypingConsole;
    public GameObject ResumeConsole;
    public GameObject LinkResume;
    public GameObject SendResume;
    public GameObject ExitResumeType;
    public GameObject StartInterview;
    public GameObject InterviewerHead;
    public GameObject InterviewerHands;
    public GameObject SendToInterviewer;
    public TMP_Text ConsoleText;
    public ChatGPT InterviewerGPT;

    public bool toggleTyping = true;
    private bool typing = false;
    private bool resumeTyping = false;

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
        if(ResumeConsole.activeSelf)
        {
            return;
        }
        if (typing)
        {
            TypingConsole.SetActive(false);
            ComputerCamera.SetActive(false);
            SendToInterviewer.SetActive(false);
            LinkResume.SetActive(true);
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
            SendToInterviewer.SetActive(true);
            LinkResume.SetActive(false);
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

    public void ToggleResumeInput()
    {

        if (resumeTyping)
        {
            ResumeConsole.SetActive(false);
            ComputerCamera.SetActive(false);
            SendResume.SetActive(false);
            ExitResumeType.SetActive(false);
            LinkResume.SetActive(true);
            StartInterview.SetActive(true);
        }
        else
        {
            ResumeConsole.SetActive(true);
            ComputerCamera.SetActive(true);
            SendResume.SetActive(true);
            ExitResumeType.SetActive(true);
            LinkResume.SetActive(false);
            StartInterview.SetActive(false);
           
        }
        resumeTyping = !resumeTyping;
    }

    public void SendInfo()
    {
        InterviewerGPT.ResponseAfterUserConsoleInput(ConsoleText.text);
        ConsoleText.text = string.Empty;
    }
}
