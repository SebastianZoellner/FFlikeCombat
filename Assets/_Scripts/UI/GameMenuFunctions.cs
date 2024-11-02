using System;
using UnityEngine;


public class GameMenuFunctions : MonoBehaviour
{
    public static event Action OnRestartLevel = delegate { };
    public static event Action OnMainMenue = delegate { };

    public void RestartLevel()
    {

        OnRestartLevel.Invoke();

        // Get the index of the currently active scene
       // int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload the currently active scene
       // SceneManager.LoadScene(sceneIndex);
    }

    public void MainMenu()
    {
        OnMainMenue.Invoke();

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
