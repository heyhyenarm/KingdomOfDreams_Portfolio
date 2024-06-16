using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamlandParticle : MonoBehaviour
{
    private ParticleSystem particle;
    private void Awake()
    {
        this.particle = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        if(this.particle.isStopped)
        {
            ObjectPooling.ReturnObject(this);
        }
    }
}
