using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuFunctions : MonoBehaviour
{
    public void RestartLevel()
    {
        // Get the index of the currently active scene
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload the currently active scene
        SceneManager.LoadScene(sceneIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
