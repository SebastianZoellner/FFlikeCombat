using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace BlackBox
{
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public class BlackBox : MonoBehaviour
    {
        public static bool LockingDisabled;
        
#if UNITY_EDITOR
        [SerializeField] private bool _locked = true; // User-visible option, to lock/unlock the Prefab
        [SerializeField] private SelectionType _selectionType = SelectionType.UseBoundingBoxes;

        private bool _canShowContents;
        private GameObject _go;
        private Transform _transform;
        private Mesh _selectionMesh;

        public bool IsLocked => _locked;
        public bool CanShowContents => _canShowContents;
        public SelectionType GetSelectionType => _selectionType; 

        public bool NeedsSelectionMesh { get; set; } = true;

        public Mesh SelectionMesh
        {
            get => _selectionMesh;
            set => _selectionMesh = value;
        }

        private void Awake()
        {
            CacheReferences();
        }

        private void CacheReferences()
        {
            _go = gameObject;
            _transform = transform;
        }

        private void OnEnable()
        {
            if(!Application.isPlaying
               && !PrefabUtility.IsPartOfAnyPrefab(_go))
            {
                if (PrefabStageUtility.GetCurrentPrefabStage() == null)
                {
                    // Just a regular GO in the scene
                    Debug.LogWarning("This is not part of a Prefab, so BlackBox functionality won't be available.", _go);
                    return;
                }
            }
            else
            {
                if(hideFlags == HideFlags.None)
                   EditorApplication.delayCall += FirstSetup; // For when using Undo, or Revert
            }
        
            PrefabStage.prefabStageOpened += OnPrefabStageOpened;
            PrefabStage.prefabStageClosing += OnPrefabStageClosing;
        }

        private void OnValidate()
        {
            NeedsSelectionMesh = true; // The user might have changed the value of _selectionType
        }

        public void FirstSetup()
        {
            // Catches the case when the user dragged an object into the scene super fast, then out again
            // At this point the script is now null, but the delayCall from OnEnable still executes
            if (this == null) return;

            // Avoids duplicating work, in case OnPrefabStageOpened triggered already
            // while the delayCall leading here was waiting to execute
            if (hideFlags != HideFlags.None) return;
            
            OnPrefabStageOpened(PrefabStageUtility.GetCurrentPrefabStage());
        }

        private void OnPrefabStageOpened(PrefabStage prefabStage)
        {
            if (CheckGlobalLocking()) return;
            
            if(_go == null) CacheReferences();
            
            if (prefabStage != null)
                if (!prefabStage.IsPartOfPrefabContents(_go)) return; // No action for Prefabs (scene instance, or previous bases) not being edited

            bool isPrefabRoot = prefabStage != null && prefabStage.prefabContentsRoot == _go;
            PrefabAssetType prefabAssetType = PrefabUtility.GetPrefabAssetType(_go);
            bool rootTypeCanShow = prefabAssetType switch
            {
                PrefabAssetType.NotAPrefab => true, // GameObject (regular root)
                PrefabAssetType.Regular => false, // Variant root
                PrefabAssetType.Model => false,
                PrefabAssetType.Variant => false,
                PrefabAssetType.MissingAsset => false,
                _ => false
            };
            bool isAddedAsOverride = PrefabUtility.IsAddedComponentOverride(this);

            _canShowContents = !_locked // Always show if unlocked
                               || (isPrefabRoot && rootTypeCanShow) // Show if is root, but not a variant (to avoid overrides on _locked)
                               || (!isPrefabRoot && rootTypeCanShow) // Show if it's a puny GameObject that is part of the hierarchy
                               || isAddedAsOverride;

            //Debug.Log($"[OP] '{_go.name}' ({_go.GetInstanceID()}) in '{_go.scene.name}' | PRoot: {isPrefabRoot} | Type: {prefabAssetType} ({rootTypeCanShow}) | Show: {_canShowContents}", _go);
            UpdateAppearance();
        }

        private void OnPrefabStageClosing(PrefabStage closedStage)
        {
            if (Application.isPlaying) return; // We don't support exiting a Prefab stage while in Play mode
            
            if (CheckGlobalLocking()) return;
            
            // Return for Prefab contents not in the active Stage (aka other levels of nesting)
            PrefabStage thisObjectsStage = PrefabStageUtility.GetPrefabStage(_go);
            PrefabStage activeStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (thisObjectsStage != activeStage) return;
        
            // No action for Prefabs edited in the previous Stage that's getting closed
            bool isPartOfCurrentStage = closedStage.IsPartOfPrefabContents(_go);
            if (isPartOfCurrentStage) return;
        
            // Determine if it's editable because it's the root of a Prefab asset (but not if it's the root of a Variant)
            bool isPrefabRoot = thisObjectsStage != null && thisObjectsStage.prefabContentsRoot == _go;
            PrefabAssetType prefabAssetType = PrefabUtility.GetPrefabAssetType(_go);
            bool rootTypeCanShow = prefabAssetType switch
            {
                PrefabAssetType.NotAPrefab => true, // GameObject (regular root)
                PrefabAssetType.Regular => false, // Variant root
                PrefabAssetType.Model => false,
                PrefabAssetType.Variant => false,
                PrefabAssetType.MissingAsset => false,
                _ => false
            };
            bool isAddedAsOverride = PrefabUtility.IsAddedComponentOverride(this);

            bool lockedByParent = false;
            if (!isPrefabRoot)
            {
                // Verify if this Prefab instance is not the root,
                // which means it can't show even if it's unlocked
                bool isInstanceRoot = PrefabUtility.IsOutermostPrefabInstanceRoot(_go);
                if(!isInstanceRoot)
                {
                    GameObject instanceRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(_go);
                    if (instanceRoot.TryGetComponent(out BlackBox blackBox)) lockedByParent = blackBox.IsLocked;
                    if (lockedByParent) return;
                }
            }

            _canShowContents = !_locked // Always show if unlocked
                               || (isPrefabRoot && rootTypeCanShow) // Show if is root, but not a variant (to avoid overrides on _locked)
                               || isAddedAsOverride;
            
            //Debug.Log($"[CL] '{_go.name}' ({_go.GetInstanceID()}) in '{_go.scene.name}' | PRoot: {isPrefabRoot} | Type: {prefabAssetType} ({rootTypeCanShow}) | Show: {_canShowContents}", _go);
            UpdateAppearance();
        }

        private bool CheckGlobalLocking()
        {
            if (LockingDisabled)
            {
                _canShowContents = true;
                UpdateAppearance();
                return true;
            }

            return false;
        }

        private void UpdateAppearance()
        {
            if (LockingDisabled) _canShowContents = true;
            UpdateOwnComponents();
            UpdateChildrenVisibility();
        }

        /// <summary>
        /// Updates components' hideFlags based on <see cref="_canShowContents"/>
        /// </summary>
        private void UpdateOwnComponents()
        {
            foreach (Component comp in _go.GetComponents<Component>())
            {
                if(comp == null) continue;
                
                if (_canShowContents) comp.hideFlags = HideFlags.None;
                else
                {
                    if (comp.GetType() != typeof(Transform) && comp != this)
                    {
                        // Hide all components that are not Transform and BlackBox
                        comp.hideFlags = HideFlags.HideInInspector;
                    }
                }
                
                // Prevent the component to be built into the game
                if (comp == this) comp.hideFlags = HideFlags.DontSaveInBuild;
            }
        }

        /// <summary>
        /// Updates the hideFlags of child GameObjects and Prefab instances, based on <see cref="_canShowContents"/>
        /// </summary>
        private void UpdateChildrenVisibility()
        {
            for (int i = 0; i < _transform.childCount; i++)
            {
                bool addedAsOverride = PrefabUtility.IsAddedGameObjectOverride(_transform.GetChild(i).gameObject);
                
                bool showObject = _canShowContents || addedAsOverride;

                _transform.GetChild(i).gameObject.hideFlags = showObject ? HideFlags.None : HideFlags.HideInHierarchy;

                // Setting the HideFlags to None on the gameObject for some reason unveils the components
                // So if the unveiled gameObject was a blackBox, we need to ask it to hide its components again
                bool hasBlackBox = _transform.GetChild(i).TryGetComponent(out BlackBox blackBoxComp);
                if (hasBlackBox)
                {
                    blackBoxComp.ForceUpdateOwnComponents();
                }
            }
        }

        /// <summary>
        /// Invoked by <see cref="SceneWatcher"/> on disabled GameObjects on scene load, to force them to hide children.
        /// Only to be used for Prefabs in scene, because it's a simplified version of FirstSetup() that doesn't take
        /// many things into account (and only uses _locked as reference)
        /// </summary>
        public void ForceUpdateAppearance()
        {
            _canShowContents = !_locked;
            CacheReferences();
            UpdateAppearance();
        }

        /// <summary>
        /// Invoked by a parent BlackBox on a child object with BlackBox, to reinstate the correct components visibility,
        /// after the parent set the hideFlags to None on the gameObject – something that messes up the hideFlags on the components.
        /// </summary>
        private void ForceUpdateOwnComponents()
        {
            CacheReferences();
            UpdateOwnComponents();
        }

        private void OnDisable()
        {
            PrefabStage.prefabStageOpened -= OnPrefabStageOpened;
            PrefabStage.prefabStageClosing -= OnPrefabStageClosing;
        }
        
        // Restore the Prefab on component removal
        private void OnDestroy()
        {
            _canShowContents = true;
            if(_go != null) UpdateAppearance();
        }
#endif
    }

    [Serializable]
    public struct RevealedProperty
    {
        [Tooltip("Optional: Give the property a new name, to be visualised on the locked Prefab in place of its original name.")]
        public string revealedAs;
        public GameObject gameObject;
        public Component component;
        public string propertyPath;
    }

    [Serializable]
    public enum SelectionType
    {
        UseRootObject,
        UseBoundingBoxes,
        Use3DMeshes,
        UseSkinnedMeshRenderers,
    }
}