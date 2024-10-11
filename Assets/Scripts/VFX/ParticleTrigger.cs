using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    public ParticleSystem[] particleSystems;  // An array if you have multiple

    public void PlayParticles()
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            Debug.Log("Playing particles");
            ps.Play();
        }
    }
}
