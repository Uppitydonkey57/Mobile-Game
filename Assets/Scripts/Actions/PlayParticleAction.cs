using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticleAction : Action
{
    public ParticleSystem particle;

    public override void PerformAction()
    {
        particle.Play();
    }
}
