using UnityEditor;
using UnityEngine;

namespace BlackBox.Editor
{
    public static class BlackBoxGizmo
    {
        [DrawGizmo(GizmoType.Selected | GizmoType.Pickable)]
        static void DrawAnotherGizmo(BlackBox script, GizmoType gizmoType)
        {
            
        }

        [DrawGizmo(GizmoType.NonSelected | GizmoType.Pickable)]
        static void DrawGizmoForMyScript(BlackBox script, GizmoType gizmoType)
        {
            if (!script.IsLocked || script.CanShowContents)
                return;
            
            switch (script.GetSelectionType)
            {
                case SelectionType.UseRootObject:
                    
                    return;
                
                case SelectionType.UseBoundingBoxes:
                case SelectionType.Use3DMeshes:
                case SelectionType.UseSkinnedMeshRenderers:

                    Mesh mesh = script.SelectionMesh;
                    if (script.NeedsSelectionMesh)
                    {
                        bool gotMesh = Utilities.CreateSelectionMesh(script.gameObject, script.GetSelectionType, out mesh);
                        if (!gotMesh)
                        {
                            // There were no MeshFilter to use, so we create a 1-unit box as selection
                            Bounds newBox = new Bounds(Vector3.zero, Vector3.one);
                            mesh = Utilities.ConvertBoundsToMesh(newBox);
                        }

                        script.NeedsSelectionMesh = false;
                        script.SelectionMesh = mesh;
                    }

                    Gizmos.color = Color.clear;
                    Gizmos.matrix = script.transform.localToWorldMatrix;
                    Gizmos.DrawMesh(mesh);
                    break;
            }
        }
    }
}