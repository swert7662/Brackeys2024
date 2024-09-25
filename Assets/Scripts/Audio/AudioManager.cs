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
        // Implementing a safer Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeSFX();
    }

    #region Volume Control

    public void SetMasterVolume(float volume) => SetVolume("Master", volume);
    public void SetMusicVolume(float volume) => SetVolume("Music", volume);
    public void SetSFXVolume(float volume) => SetVolume("SFX", volume);

    public float GetMasterVolume() => GetVolume("Master");
    public float GetMusicVolume() => GetVolume("Music");
    public float GetSFXVolume() => GetVolume("SFX");

    private void SetVolume(string parameterName, float volume)
    {
        audioMixer.SetFloat(parameterName, Mathf.Log10(volume) * 20);
    }

    private float GetVolume(string parameterName)
    {
        if (audioMixer.GetFloat(parameterName, out float value))
        {
            return Mathf.Pow(10, value / 20);
        }
        return 0f; // Return 0 if the parameter is not found
    }

    #endregion

    private void InitializeSFX()
    {
        foreach (var group in soundEffectGroups)
        {
            foreach (var sfx in group.soundEffects)
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

    #region Sound Effect Playback

    public void PlaySFX(string groupName, string clipName)
    {
        var sfx = GetSoundEffect(groupName, clipName);
        if (sfx != null)
        {
            RandomizePitchVolume(sfx.Group, sfx);
            sfx.source.Play();
        }
    }

    public void PlaySFX(string groupName)
    {
        var sfx = GetRandomSoundEffect(groupName);
        if (sfx != null)
        {
            RandomizePitchVolume(sfx.Group, sfx);
            sfx.source.Play();
        }
    }

    public SoundEffect GetSFX(string groupName)
    {
        return GetRandomSoundEffect(groupName);
    }

    #endregion

    #region Ambient Sound Effects

    public void AmbientSFXFadeIn(string groupName, string clipName, float fadeDuration = 1.5f)
    {
        var sfx = GetSoundEffect(groupName, clipName);
        if (sfx != null)
        {
            sfx.source.Play();
            StartCoroutine(FadeIn(sfx.source, fadeDuration));
        }
    }

    public void AmbientSFXFadeOut(string groupName, string clipName, float fadeDuration = 1.5f)
    {
        var sfx = GetSoundEffect(groupName, clipName);
        if (sfx != null)
        {
            StartCoroutine(FadeOut(sfx.source, fadeDuration));
        }
    }

    #endregion

    #region Coroutines

    private IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        float currentTime = 0f;
        audioSource.volume = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, currentTime / duration);
            yield return null;
        }
        audioSource.volume = 1f;
    }

    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, currentTime / duration);
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    #endregion

    #region Helper Methods

    private SoundEffectGroup GetSoundEffectGroup(string groupName)
    {
        var group = Array.Find(soundEffectGroups, g => g.groupName == groupName);
        if (group == null)
        {
            Debug.LogWarning($"SoundEffectGroup {groupName} not found");
        }
        return group;
    }

    private SoundEffect GetSoundEffect(string groupName, string clipName)
    {
        var group = GetSoundEffectGroup(groupName);
        if (group != null)
        {
            var sfx = Array.Find(group.soundEffects, s => s.name == clipName);
            if (sfx == null)
            {
                Debug.LogWarning($"Sound {clipName} not found in group {groupName}");
            }
            else
            {
                sfx.Group = group; // Assign group reference for randomization
            }
            return sfx;
        }
        return null;
    }

    private SoundEffect GetRandomSoundEffect(string groupName)
    {
        var group = GetSoundEffectGroup(groupName);
        if (group != null && group.soundEffects.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, group.soundEffects.Length);
            var sfx = group.soundEffects[randomIndex];
            sfx.Group = group; // Assign group reference for randomization
            return sfx;
        }
        Debug.LogWarning($"SoundEffectGroup {groupName} not found or contains no sounds");
        return null;
    }

    public AudioClip GetRandomClipFromGroup(string groupName)
    {
        SoundEffectGroup group = GetSoundEffectGroup(groupName);
        if (group != null && group.soundEffects.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, group.soundEffects.Length);
            SoundEffect sfx = group.soundEffects[randomIndex];
            return sfx.clip;
        }
        Debug.LogWarning($"SoundEffectGroup {groupName} not found or contains no sounds");
        return null;
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

    #endregion
}
