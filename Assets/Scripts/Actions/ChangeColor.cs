using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChangeColor : Action
{
    public Color changeColor;

    public float duration;

    SpriteRenderer rend;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    public override void PerformAction()
    {
        rend.DOColor(changeColor, duration);
    }
}
