using Meta.Voice.Samples.Dictation;
using Meta.WitAi.Dictation;
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
        if(clearOnNextToggle)
        {
            transcriptionHandler.Clear();
            clearOnNextToggle = false;
        }
        
        dictationActivation.ToggleActivation();
        if(DictationActivation.ReadyToSend())
        {
            clearOnNextToggle = true;
        }
    }
}
