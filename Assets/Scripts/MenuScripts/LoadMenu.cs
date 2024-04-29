using UnityEngine;


public class LoadMenu : MonoBehaviour
{
    [SerializeField] public GameObject menuPanel; // Assign this in the Unity Editor
    [SerializeField] public Transform playerCamera; // Assign the player's camera or head transform here

    public float distanceInFront = 2.0f; // How far in front of the player to spawn the object

    void Update()
    {
        // Check if the menu button on the left controller is pressed
        if (OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.LTouch))
        {
            TeleportObjectInFrontOfPlayer();
        }
    }

    private void TeleportObjectInFrontOfPlayer()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Player camera is not assigned.");
            return;
        }

        // Calculate the position in front of the player
        Vector3 positionInFront = playerCamera.position + playerCamera.forward * distanceInFront;
        Quaternion orientation = Quaternion.LookRotation(playerCamera.forward);

        if (menuPanel.activeSelf)
        {
            menuPanel.SetActive(false);
        }
        else
        {
            menuPanel.SetActive(true);
            menuPanel.transform.position = positionInFront;
            menuPanel.transform.rotation = orientation;
        }
    }
}
