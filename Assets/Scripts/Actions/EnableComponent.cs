using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableComponent : Action
{
    public MonoBehaviour monoBehaviour;

    public bool value;

    public override void PerformAction()
    {
        monoBehaviour.enabled = value;
    }
}
