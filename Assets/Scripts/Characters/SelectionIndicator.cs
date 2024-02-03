using UnityEngine;

public class SelectionIndicator : MonoBehaviour
{
    [SerializeField] Material selectedMaterial;
    private Material oldMaterial;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void SetSelected()
    {
        oldMaterial = meshRenderer.material;
        meshRenderer.material = selectedMaterial;
    }

    public void SetDeselected()
    {
        meshRenderer.material = oldMaterial;
    }
}
