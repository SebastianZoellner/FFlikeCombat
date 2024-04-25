using TMPro;
using UnityEngine;

public class HoverTipManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tipText;
    [SerializeField] RectTransform tipWindow;

    [SerializeField] int tipWindowWidth = 300;

    private void OnEnable()
    {
        HoverTip.OnMouseLoseFocus += HideTip;
        HoverTip.OnMouseHover += ShowTip;
    }
    private void Start()
    {
        HideTip();
    }

    private void OnDisable()
    {
        HoverTip.OnMouseLoseFocus -= HideTip;
        HoverTip.OnMouseHover -= ShowTip;
    }


    private void ShowTip(string tip,Vector2 mousePos)
    {
       
        //Debug.Log(tipText.text + "    " + tipText.preferredWidth + " " + tipText.preferredHeight);
        tipText.text = tip;
        //Debug.Log(tipText.text+"    "+tipText.preferredWidth + " " + tipText.preferredHeight);
        tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > tipWindowWidth ? tipWindowWidth : tipText.preferredWidth, tipText.preferredHeight);
        tipWindow.gameObject.SetActive(true);
        tipWindow.transform.position = new Vector2(mousePos.x + tipWindow.sizeDelta.x * 0.75f,mousePos.y);
    }

    private void HideTip()
    {
        tipWindow.gameObject.SetActive(false);
        tipText.text = default;
    }
}
