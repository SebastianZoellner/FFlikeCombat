using UnityEngine;
using HighlightPlus;

public class SelectionIndicator : MonoBehaviour
{
    private HighlightEffect highlightEffect;

    private void Awake()
    {
        highlightEffect = GetComponent<HighlightEffect>();
    }

    public void SetSelected()
    {
        highlightEffect.highlighted = true;
    }

    public void SetDeselected()
    {
       highlightEffect.highlighted=false;
    }
}
