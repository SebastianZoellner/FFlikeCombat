using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public event Action<string> OnSceneLoaded = delegate { };
    private Dictionary<string, AsyncOperation> loadDictionary;


    private void Awake()
    {
        loadDictionary = new Dictionary<string, AsyncOperation>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    public void LoadScene(string sceneName, bool additive)
    {
        Debug.Log("Loading " + sceneName);
        if (additive)
            loadDictionary[sceneName] = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        else
            loadDictionary[sceneName] = SceneManager.LoadSceneAsync(sceneName);
    }

    public float GetLoadProgress(string name) => loadDictionary[name].progress;

    public void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    public void SetActiveScene(string sceneName) => SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnSceneLoaded.Invoke(scene.name);
        if (loadDictionary.ContainsKey(scene.name))
            loadDictionary.Remove(scene.name);
    }

    
}
