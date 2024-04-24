using Meta.Voice.Samples.Dictation;
using Meta.WitAi.Dictation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRecording : MonoBehaviour
{
    private DictationActivation dictationActivation;
    [SerializeField] private TranscriptionHandler transcriptionHandler;
    private bool clearOnNextToggle;
    void Start()
    {
        dictationActivation = this.GetComponent<DictationActivation>();
    }

    public void ToggleRecord()
    {
        print("Toggle record");
        if(clearOnNextToggle)
        {
            transcriptionHandler.Clear();
            clearOnNextToggle = false;
        }
        
        dictationActivation.ToggleActivation();
        if(dictationActivation.ReadyToSend())
        {
            clearOnNextToggle = true;
        }
    }
}
