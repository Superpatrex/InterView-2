using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Oculus.Interaction;

public class TurnModeToggle : MonoBehaviour
{
    [SerializeField] public GameObject turnGameObject; // The GameObject on which the turn providers are attached

    void Start()
    {
        
    }
    
    public void enableSnapTurn()
    {
        ActionBasedSnapTurnProvider snapTurnProvider = turnGameObject.GetComponent<ActionBasedSnapTurnProvider>();
        ActionBasedContinuousTurnProvider continuousTurnProvider = turnGameObject.GetComponent<ActionBasedContinuousTurnProvider>();
        // Toggle the enabled state of both providers
        snapTurnProvider.enabled = true;
        continuousTurnProvider.enabled = false;
    }
    public void enableContinuousTurn()
    {
        ActionBasedSnapTurnProvider snapTurnProvider = turnGameObject.GetComponent<ActionBasedSnapTurnProvider>();
        ActionBasedContinuousTurnProvider continuousTurnProvider = turnGameObject.GetComponent<ActionBasedContinuousTurnProvider>();
        // Toggle the enabled state of both providers
        continuousTurnProvider.enabled = true;
        snapTurnProvider.enabled = false;
    }
}
