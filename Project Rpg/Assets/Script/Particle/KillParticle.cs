using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillParticle : MonoBehaviour
{
    public ParticleSystem System;

    void Update()
    {
        if (System.isStopped)
            Destroy(this.gameObject);
    }
}
