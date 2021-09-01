using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : Action
{
    public GameObject activateObject;
    public bool isActive;

    public override void PerformAction()
    {
        if (isActive)
        {
            activateObject.SetActive(isActive);
        }
    }
}
