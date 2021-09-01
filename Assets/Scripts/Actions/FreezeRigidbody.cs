using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRigidbody : Action
{
    public Rigidbody2D objectRigidbody;

    public bool freezeX;
    public bool freezeY;
    public bool freezeRotation = true;

    public override void PerformAction()
    {
        RigidbodyConstraints2D xConstraint = (freezeX ? RigidbodyConstraints2D.FreezePositionX : RigidbodyConstraints2D.None);
        RigidbodyConstraints2D yConstraint = (freezeY ? RigidbodyConstraints2D.FreezePositionY : RigidbodyConstraints2D.None);
        RigidbodyConstraints2D rotationConstraint = (freezeRotation ? RigidbodyConstraints2D.FreezeRotation : RigidbodyConstraints2D.None);

        objectRigidbody.constraints = xConstraint | yConstraint | rotationConstraint;
    }
}
