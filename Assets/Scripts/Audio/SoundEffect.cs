using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class SoundEffect
{
    public string name;

    public AudioClip clip;

    public AudioMixerGroup mixerGroup;

    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;

    public bool loop = false;
    public bool playOnAwake = false;

    [HideInInspector]
    public AudioSource source;

    // Add this property to reference the parent group
    [HideInInspector]
    public SoundEffectGroup Group;
}
