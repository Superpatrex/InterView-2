using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using TMPro;
using OpenAI;
using UnityEngine.UI;

public class InterviewSelectionController : MonoBehaviour
{
    [SerializeField] private ChatGPT chatGPT;
    [SerializeField] private Selected initiallySelected;
    private Selected currentlySelected;
    
    List<GameObject> children;

    void Start()
    {
        children = new List<GameObject>();
        gameObject.GetChildGameObjects(children);
        initiallySelected.selected = true;
    }
    public void UpdateSelection(string jobRole, Selected button)
    {
        if(currentlySelected != null)
        {
            currentlySelected.selected = false;
        }
        chatGPT.ChangeJobRole(jobRole);
        currentlySelected = button;
       
    }
}
