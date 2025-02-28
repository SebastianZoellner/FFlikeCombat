using System.Collections;
using UnityEngine;

public class FXBatAttack : MonoBehaviour
{
    [SerializeField] float errodeRate = 0.03f;
    [SerializeField] float erodeRefreshRate = 0.01f;
    [SerializeField] float errodeDelay = 1.25f;
    [SerializeField] SkinnedMeshRenderer[] batRendererArray;

    private void Start()
    {
        StartCoroutine(ErodeObject());
    }

    IEnumerator ErodeObject()
    {
        //yield return new WaitForSeconds(errodeDelay);
        float t = 1;
        while (t > 0)
        {
            t -= errodeRate;
            foreach (SkinnedMeshRenderer smr in batRendererArray)
                smr.material.SetFloat("_erode", t);
            yield return new WaitForSeconds(erodeRefreshRate);
        }
    }
}
