using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHolder : MonoBehaviour
{
    public void ChangeParticlesState(string childParticlesHolderName, bool play)
    {
        Transform particlesHolder = transform.Find(childParticlesHolderName);
        if (particlesHolder != null)
        {
            ParticleSystem[] particles = particlesHolder.GetComponentsInChildren<ParticleSystem>();
            if (particles != null)
            {
                foreach (ParticleSystem particle in particles)
                {
                    if (play)
                    {
                        particle.Play();
                    }
                    else
                    {
                        particle.Stop();
                    }
                }
            }
            else
            {
                Debug.LogError("PARTICLEHOLDER: " + childParticlesHolderName + " particles not found");
            }
        }
        else
        {
            Debug.LogError("PARTICLEHOLDER: " + childParticlesHolderName + " GO not found");
        }
    }

    public void PlayParticles(string childParticlesHolderName)
    {
        ChangeParticlesState(childParticlesHolderName, true);
    }

    public void StopParticles(string childParticlesHolderName)
    {
        ChangeParticlesState(childParticlesHolderName, false);
    }

}
