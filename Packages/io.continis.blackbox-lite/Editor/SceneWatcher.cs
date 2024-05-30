using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using UEditor = UnityEditor.Editor;

namespace BlackBox.Editor
{
    public static class SceneWatcher
    {
        [InitializeOnLoadMethod]
        private static void Initialise()
        {
            EditorSceneManager.sceneOpened += OnSceneOpened;
            Selection.selectionChanged += () => CheckSelection(true);
        }

        public static void CheckSelection(bool asDelayed)
        {
            if (Selection.activeGameObject == null) return;
            if (BlackBoxSettings.DisableLocking.value) return;
            
            // TODO: Improve selection nulling for situations where there are two BlackBox components inside each other
            if (PrefabUtility.IsPartOfAnyPrefab(Selection.activeGameObject))
            {
                GameObject root = PrefabUtility.GetOutermostPrefabInstanceRoot(Selection.activeGameObject);
                if (root == null || Selection.activeGameObject == root) return; // root is null for Prefab assets
                
                if (root.TryGetComponent(out BlackBox component))
                {
                    if (!component.IsLocked) return;
                    if (PrefabUtility.GetPrefabAssetType(root) != PrefabAssetType.Model)
                    {
                        if(asDelayed) EditorApplication.delayCall += () => Selection.activeGameObject = component.gameObject;
                        else Selection.activeGameObject = component.gameObject;
                    }
                }
            }
        }

        private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            if (BlackBoxSettings.DisableLocking.value) return;
            
            BlackBox[] blackBoxes = Object.FindObjectsByType<BlackBox>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (BlackBox blackBox in blackBoxes) if (!blackBox.gameObject.activeSelf) blackBox.ForceUpdateAppearance();
        }

        public static void UpdateAllPrefabsInScene()
        {
            BlackBox[] blackBoxes = Object.FindObjectsByType<BlackBox>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (BlackBox blackBox in blackBoxes) blackBox.ForceUpdateAppearance();
        }

        private struct PropertyInfoStruct
        {
            public BlackBox blackBox;
            public GameObject go;
            public Component comp;
            public SerializedProperty serializedProp;

            public PropertyInfoStruct(BlackBox blackBox, GameObject go, Component comp, SerializedProperty serializedProp)
            {
                this.blackBox = blackBox;
                this.serializedProp = serializedProp;
                this.go = go;
                this.comp = comp;
            }
        }
    }
}