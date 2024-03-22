using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class FireballProjectile : BattleProjectile
{
    [field: SerializeField] public ParticleSystem FireballParticles { get; private set; }
    [field: SerializeField] public ParticleSystem ExplosionParticles { get; private set; }

    public void SetRemainingParticlesLifetime(ParticleSystem givenParticleSystem, float remainingLifetime) //sets givenParticleSystem and its children
    {
        ParticleSystem[] allParticleSystems = givenParticleSystem.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem particleSystem in allParticleSystems)
        {
            Particle[] particles = new Particle[particleSystem.main.maxParticles];
            int currentAmount = particleSystem.GetParticles(particles);

            // Change only the particles that are alive
            for (int i = 0; i < currentAmount; i++)
            {
                particles[i].remainingLifetime = remainingLifetime;
            }

            particleSystem.SetParticles(particles, currentAmount);
        }
    }

    public IEnumerator DestroyProjectile()
    {
        while (FireballParticles != null || ExplosionParticles != null)
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}
