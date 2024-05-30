using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BlackBox.Editor
{
    [CustomEditor(typeof(BlackBox))]
    public class BlackBoxEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset internalEditor;
        [SerializeField] private VisualTreeAsset externalEditor;
        
        private SerializedProperty _lockedProp;
        private BlackBox _comp;
        private GameObject _go;

        private bool _isDisplayingExternalEditor;
        private Button _hideShowButton;
        private VisualElement _inspector;

        public static BlackBoxEditor CurrentEditor;
        
        public void Awake()
        {
            CacheReferences();
        }

        private void CacheReferences()
        {
            _comp = (BlackBox)target;
            _go = _comp.gameObject;
            _lockedProp = serializedObject.FindProperty("_locked");
        }

        private void OnEnable()
        {
            CurrentEditor = this;
        }

        private void OnDisable()
        {
            CurrentEditor = null;
        }
        
        public override VisualElement CreateInspectorGUI()
        {
            _inspector = new();
            if(_lockedProp == null) CacheReferences();
            
            // If the Prefab has been dragged and dropped into the scene,
            // Unity forces the hideFlags of the GameObject (and its components) to None on release,
            // which reveals all components.
            // This if is meant to catch that case, and force the object to update itself
            if(_comp.hideFlags == HideFlags.None) _comp.FirstSetup();
            
            // Determine if it's the Internal or External editor
            PrefabStage activeStage = PrefabStageUtility.GetCurrentPrefabStage();
            bool isPrefabRoot = activeStage != null && activeStage.prefabContentsRoot == _go;
            PrefabAssetType prefabAssetType = PrefabUtility.GetPrefabAssetType(_go);
            bool isVariant = prefabAssetType != PrefabAssetType.NotAPrefab;
            bool isPrefabModel = prefabAssetType == PrefabAssetType.Model;
            bool isAsset = AssetDatabase.Contains(_go);
            bool isAddedAsOverride = PrefabUtility.IsAddedComponentOverride(_comp);
            
            // Debug.Log($"Prefab root: {isPrefabRoot} | Variant: {isVariant} | P.Model: {isPrefabModel} | Asset: {isAsset} | Comp is override: {isAddedAsOverride}");

            if ((isPrefabRoot && (!isVariant || isPrefabModel)) || isAddedAsOverride || isAsset)
            {
                // Prefab Mode Inspector
                internalEditor.CloneTree(_inspector);
                
                DisplayDisabledMessage();
            }
            else
            {
                // Instance Inspector
                externalEditor.CloneTree(_inspector);
                
                DisplayDisabledMessage();
                DisplayLockedMessage();
            }

            return _inspector;
        }
        
        private void OnSceneGUI() => SceneWatcher.CheckSelection(false);

        #region External Inspector

        private void DisplayLockedMessage()
        {
            Label lockedLabel = _inspector.Q<Label>("LockedMessage");
            if (!BlackBoxSettings.DisableLocking.value)
            {
                lockedLabel.text = _comp.IsLocked ? "Prefab is locked" : "Prefab is unlocked";
            }
            else
            {
                lockedLabel.style.display = DisplayStyle.None;
            }
        }
        
        #endregion

        #region Internal and External

        private void DisplayDisabledMessage()
        {
            if (BlackBoxSettings.DisableLocking.value)
                _inspector.Insert(0,
                    new HelpBox(
                        "Prefab locking has been disabled globally.\nChange it in Project Settings > Black Box.",
                        HelpBoxMessageType.Warning));
        }

        #endregion
    }
}