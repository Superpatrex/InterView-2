using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Selected : MonoBehaviour
{
    
    [SerializeField] public TMP_Text jobRole;
    [SerializeField] public InterviewSelectionController interviewSelectionController;
    [NonSerialized] public bool selected = false;
    
    private Button thisButton;
    private ColorBlock cb;
    private Color darkGrey = new Color(.3f, .3f, .3f);
    
    private bool selectBool = false;
    [Tooltip("ONLY HAVE ONE SELECTED AT A TIME")]
    public void Start()
    {
        thisButton = GetComponent<Button>();
        cb = thisButton.colors;
    }
    public void Select()
    {
        selected = true;
    }
    public void Update()
    {
        if(selected!= selectBool)
        {
            selectBool = selected;
            if (selected)
            {
                interviewSelectionController.UpdateSelection(jobRole.text, this);
                cb.normalColor = Color.green;
                thisButton.colors = cb;
            }
            else
            {
                cb.normalColor = darkGrey;
                thisButton.colors = cb;
            }
        }
    }
}
