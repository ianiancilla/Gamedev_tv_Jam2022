using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ProjectileShooter : MonoBehaviour
{
    // cache
    ParticleSystem particleSystem;
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        EnableShooting(false);
    }

    public void OnParticleCollision(GameObject other)
    {
        Destroy(other);
    }

    public void EnableShooting(bool newState)
    {
        if (newState)
        {
            particleSystem.Play();
        }
        else
        {
            particleSystem.Stop();
        }
    }

}
