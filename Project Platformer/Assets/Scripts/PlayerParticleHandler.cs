using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleHandler : MonoBehaviour
{
    [SerializeField] private Particle[] _particles;

    public void PlayParticle(string name, Vector3 position, Quaternion rotation, Transform parent)
    {
        for (int i = 0; i < _particles.Length; i++)
        {
            if (_particles[i].particleName == name)
            {
                ParticleSystem newParticle = Instantiate(_particles[i].particle, position, rotation, parent);
                newParticle.Play();
                return;
            }
        }
        Debug.LogWarning("PlayerParticleHandler: Particle not found in list: " + name);
    }
}
