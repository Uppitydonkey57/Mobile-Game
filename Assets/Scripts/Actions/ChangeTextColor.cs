using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChangeTextColor : Action
{
    public TextMeshProUGUI tmpText;
    public Text text;

    public float duration;

    public Color endColor;

    public override void PerformAction()
    {
        if (text != null)
        {
            text.CrossFadeColor(endColor, duration, false, true);
        }

        if (tmpText != null)
        {
            tmpText.CrossFadeColor(endColor, duration, false, true);
        }
    }
}
