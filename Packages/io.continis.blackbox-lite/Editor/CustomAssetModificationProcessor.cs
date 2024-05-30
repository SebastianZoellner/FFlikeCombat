using UnityEditor;
using UnityEngine;

namespace BlackBox.Editor
{
    public class CustomAssetModificationProcessor : AssetModificationProcessor
    {
        static void OnWillCreateAsset(string assetName)
        {
            if (!BlackBoxSettings.AutoAddToPrefabs.value &&
                !BlackBoxSettings.AutoAddToVariants.value)
                return;
            
            if (assetName.EndsWith(".prefab"))
                EditorApplication.delayCall += () => DelayCall(assetName);
        }

        private static void DelayCall(string assetName)
        {
            GameObject prefabRoot = AssetDatabase.LoadMainAssetAtPath(assetName) as GameObject;
            PrefabAssetType prefabAssetType = PrefabUtility.GetPrefabAssetType(prefabRoot);

            switch (prefabAssetType)
            {
                case PrefabAssetType.Variant:
                {
                    if (!BlackBoxSettings.AutoAddToVariants.value) return;
                    break;
                }
                case PrefabAssetType.Regular:
                {
                    if (!BlackBoxSettings.AutoAddToPrefabs.value) return;
                    break;
                }
            }

            if(!prefabRoot!.TryGetComponent(out BlackBox _))
            {
                BlackBox addComponent = prefabRoot.AddComponent<BlackBox>();
                while (UnityEditorInternal.ComponentUtility.MoveComponentUp(addComponent))
                {
                    
                }
                PrefabUtility.SavePrefabAsset(prefabRoot);
            }
        }
    }
}