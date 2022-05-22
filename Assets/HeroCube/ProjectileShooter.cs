using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ProjectileShooter : MonoBehaviour
{
    // cache
    ParticleSystem partSystem;
    void Start()
    {
        partSystem = GetComponent<ParticleSystem>();
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
            partSystem.Play();
        }
        else
        {
            partSystem.Stop();
        }
    }

}
