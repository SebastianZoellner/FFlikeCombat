using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSetup : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        Debug.Log("Loaded?");
    }
}
