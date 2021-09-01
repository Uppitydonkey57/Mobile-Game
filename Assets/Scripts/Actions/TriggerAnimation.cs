using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : Action
{
    public Animator animator;

    public string triggerName;

    public override void PerformAction()
    {
        animator.SetTrigger(triggerName);
    }


}
