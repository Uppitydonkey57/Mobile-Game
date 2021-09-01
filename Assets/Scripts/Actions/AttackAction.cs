using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action
{
    public Weapon weapon;

    public override void PerformAction()
    {
        weapon.Attack();
    }
}
