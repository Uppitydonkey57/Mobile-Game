using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentPlayer : Action
{
    public Transform objectTransform;

    public override void PerformAction()
    {
        FindObjectOfType<PlayerController>().transform.SetParent(objectTransform);
    }
}
