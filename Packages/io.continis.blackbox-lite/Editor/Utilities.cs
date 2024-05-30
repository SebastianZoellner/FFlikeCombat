using System;
using UnityEngine;

namespace BlackBox.Editor
{
    public static class Utilities
    {
        public static bool CreateSelectionMesh(GameObject gameObject, SelectionType scriptSelectionType, out Mesh result)
        {
            result = new Mesh();
            CombineInstance[] combine;

            if (scriptSelectionType is SelectionType.UseBoundingBoxes or SelectionType.Use3DMeshes)
            {
                MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();
                if (meshFilters.Length == 0) return false;
        
                combine = new CombineInstance[meshFilters.Length];
                for (int i = 0; i < meshFilters.Length; i++)
                {
                    Mesh mesh = meshFilters[i].sharedMesh;
                    if (mesh == null) continue;
            
                    combine[i].mesh = scriptSelectionType == SelectionType.Use3DMeshes ? mesh : ConvertBoundsToMesh(mesh.bounds);
                    combine[i].transform = gameObject.transform.worldToLocalMatrix * meshFilters[i].transform.localToWorldMatrix;
                }
            }
            else if(scriptSelectionType == SelectionType.UseSkinnedMeshRenderers)
            {
                SkinnedMeshRenderer[] skinnedMeshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
                if (skinnedMeshRenderers.Length == 0) return false;
        
                combine = new CombineInstance[skinnedMeshRenderers.Length];
                for (int i = 0; i < skinnedMeshRenderers.Length; i++)
                {
                    Mesh mesh = new Mesh();
                    skinnedMeshRenderers[i].BakeMesh(mesh, true);
                    if (mesh == null) continue;
                    
                    combine[i].mesh = mesh;
                    combine[i].transform = gameObject.transform.worldToLocalMatrix * skinnedMeshRenderers[i].transform.localToWorldMatrix;
                }
            }
            else
            {
                throw new NotImplementedException("Selection type shouldn't be this type.");
            }

            result.CombineMeshes(combine);
            return combine.Length > 0;
        }

        public static Mesh ConvertBoundsToMesh(Bounds bounds)
        {
            Vector3[] vertices = new Vector3[8];
            vertices[0] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z);
            vertices[1] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z);
            vertices[2] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, bounds.extents.z);
            vertices[3] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
            vertices[4] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, bounds.extents.z);
            vertices[5] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z);
            vertices[6] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, bounds.extents.z);
            vertices[7] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, -bounds.extents.z);

            int[] triangles = new int[]
            {
                0, 2, 1, // Front
                1, 2, 3,
                4, 5, 6, // Back
                5, 7, 6,
                0, 1, 4, // Top
                1, 5, 4,
                2, 6, 3, // Bottom
                3, 6, 7,
                0, 4, 2, // Left
                2, 4, 6,
                1, 3, 5, // Right
                3, 7, 5
            };

            Vector3[] normals = new Vector3[vertices.Length];
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int index1 = triangles[i];
                int index2 = triangles[i + 1];
                int index3 = triangles[i + 2];
                Vector3 side1 = vertices[index2] - vertices[index1];
                Vector3 side2 = vertices[index3] - vertices[index1];
                Vector3 normal = Vector3.Cross(side1, side2).normalized;

                normals[index1] += normal;
                normals[index2] += normal;
                normals[index3] += normal;
            }

            for (int i = 0; i < normals.Length; i++) normals[i] = normals[i].normalized;

            Mesh mesh = new();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;

            return mesh;
        }
    }
}