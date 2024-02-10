using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSetup : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        
    }
}
