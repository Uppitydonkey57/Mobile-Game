using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateActor : Action
{
    public Actor actor;

    public override void PerformAction()
    {
        Debug.Log("Active");
        actor.Activate();
    }
}
