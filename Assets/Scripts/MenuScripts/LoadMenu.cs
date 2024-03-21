using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene management

public class MenuOpener : MonoBehaviour
{
    void Update()
    {
        // Check if the menu button on the left controller is pressed
        if (OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.LTouch))
        {
            // Load the menu scene
            SceneManager.LoadScene("Menu");
        }
    }
}
