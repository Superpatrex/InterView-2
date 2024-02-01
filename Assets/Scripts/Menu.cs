using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    // Called when we click the "Play" button.
    public void OnPlayButton ()
    {
        SceneManager.LoadScene(1);
    }
    public void OnSettingsButton ()
    {
        // if we want to add fancy code logic to switch
    }
    public void OnBackButton ()
    {
        // if we want to add fancy code logic to switch
        // should take u back to the previous state of the menu board
        // switch to settings
    }
    // Called when we click the "Quit" button.
    public void OnQuitButton ()
    {
        Application.Quit();
    }
}