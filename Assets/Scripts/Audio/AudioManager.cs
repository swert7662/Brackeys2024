using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System;
using UnityEngine.Rendering.PostProcessing;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource musicSource; // AudioSource for music
    [SerializeField] private AudioSource sfxSource;   // AudioSource for SFX
    [SerializeField] private SoundEffectGroup[] soundEffectGroups;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // Assign the AudioSources to each SoundEffect
            InitializeSFX();
        }
        else
        {
            Destroy(gameObject);
        }

        
    }    

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    public float GetMasterVolume()
    {
        float value;
        audioMixer.GetFloat("Master", out value);
        return Mathf.Pow(10, value / 20);
    }

    public float GetMusicVolume()
    {
        float value;
        audioMixer.GetFloat("Music", out value);
        return Mathf.Pow(10, value / 20);
    }

    public float GetSFXVolume()
    {
        float value;
        audioMixer.GetFloat("SFX", out value);
        return Mathf.Pow(10, value / 20);
    }
    private void InitializeSFX()
    {
        foreach (SoundEffectGroup group in soundEffectGroups)
        {
            foreach (SoundEffect sfx in group.soundEffects)
            {
                sfx.source = gameObject.AddComponent<AudioSource>();
                sfx.source.clip = sfx.clip;
                sfx.source.outputAudioMixerGroup = sfx.mixerGroup;

                sfx.source.volume = sfx.volume;
                sfx.source.pitch = sfx.pitch;

                sfx.source.loop = sfx.loop;
                sfx.source.playOnAwake = sfx.playOnAwake;
            }
        }
    }
    // Play a sound effect from a specified group and with a specified name
    public void PlaySFX(string groupName, string clipName)
    {
        SoundEffectGroup group = Array.Find(soundEffectGroups, g => g.groupName == groupName);

        if (group != null)
        {
            SoundEffect sfx = Array.Find(group.soundEffects, sfx => sfx.name == clipName);
            if (sfx != null)
            {
                RandomizePitchVolume(group, sfx);
                sfx.source.Play();
            }
            else { Debug.LogWarning($"Sound {clipName} not found in group {groupName}"); }
        }
        else { Debug.LogWarning($"SoundEffectGroup {groupName} not found"); }
    }

    // Overload to play a random sound from a specified group
    public void PlaySFX(string groupName)
    {
        SoundEffectGroup group = Array.Find(soundEffectGroups, g => g.groupName == groupName);

        if (group != null && group.soundEffects.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, group.soundEffects.Length);
            SoundEffect sfx = group.soundEffects[randomIndex];

            if (sfx != null)
            {
                RandomizePitchVolume(group, sfx);
                sfx.source.Play();
            }
        }
        else
        { Debug.LogWarning($"SoundEffectGroup {groupName} not found or contains no sounds"); }
    }
    private void RandomizePitchVolume(SoundEffectGroup group, SoundEffect sfx)
    {
        if (group.RandomizeGroupVolume)
        {
            sfx.source.volume = UnityEngine.Random.Range(group.volumeRange.x, group.volumeRange.y);
        }

        if (group.RandomizeGroupPitch)
        {
            sfx.source.pitch = UnityEngine.Random.Range(group.pitchRange.x, group.pitchRange.y);
        }
    }


    /*

    /// <summary>
    /// Plays a sound effect with a specified pitch variation.
    /// </summary>
    /// <param name="clip">The AudioClip to play.</param>
    /// <param name="pitch">The pitch to apply to the sound.</param>
    public void PlaySFX(AudioClip clip, float pitch)
    {
        if (clip == null) return;

        StartCoroutine(PlaySFXWithPitch(clip, pitch));
    }

    private IEnumerator PlaySFXWithPitch(AudioClip clip, float pitch)
    {
        // Create a temporary GameObject to hold the AudioSource
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.SetParent(this.transform);

        // Add an AudioSource component
        AudioSource tempSource = tempGO.AddComponent<AudioSource>();
        tempSource.outputAudioMixerGroup = sfxSource.outputAudioMixerGroup; // Ensure it uses the SFX mixer group
        tempSource.pitch = pitch;
        tempSource.clip = clip;
        tempSource.Play();

        // Wait for the clip to finish playing
        yield return new WaitForSeconds(clip.length / tempSource.pitch);

        // Destroy the temporary GameObject
        Destroy(tempGO);
    }
    */
}
