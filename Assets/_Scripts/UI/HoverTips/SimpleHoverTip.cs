using UnityEngine;

public class SimpleHoverTip : HoverTip
{
    [TextArea]
    [SerializeField] string simpleTip;

    protected override string GetTip()
    {
        return simpleTip;
    }
}
