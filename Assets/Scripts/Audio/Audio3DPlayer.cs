using UnityEngine;
using System.Collections.Generic;

public class Audio3DPlayer : MonoBehaviour
{
    [Header("Playback Settings")]
    public float interval = 5f;            // Interval between sound effects
    public string audioGroup;              // The name of the audio group to pull sounds from

    [Header("AudioSource Settings")]
    public float dopplerLevel = 1f;        // Doppler effect level
    public float spread = 0f;              // Stereo spread angle (0 to 360)
    public float minDistance = 1f;         // Minimum distance for the sound to be heard
    public float maxDistance = 500f;       // Maximum distance at which the sound stops attenuating

    [Header("Volume and Pitch Ranges")]
    [Tooltip("Random volume range (x = min, y = max)")]
    public Vector2 volumeRange = new Vector2(1f, 1f); // Volume range (0.0 to 1.0)
    [Tooltip("Random pitch range (x = min, y = max)")]
    public Vector2 pitchRange = new Vector2(1f, 1f);  // Pitch range (0.1 to 3.0)

    [Header("AudioSource Pool Settings")]
    public int initialPoolSize = 3;        // Starting number of AudioSources in the pool
    public int maxPoolSize = 10;           // Maximum number of AudioSources allowed

    private float timer;                   // Internal timer to track the interval
    private List<AudioSource> audioSourcePool = new List<AudioSource>();

    private void Start()
    {
        InitializeAudioSourcePool();
        timer = interval; // Initialize the timer
    }

    private void Update()
    {
        // Count down the timer
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // Play a new sound and reset the timer
            PlayRandomSound();
            timer = interval; // Reset the timer
        }
    }

    private void InitializeAudioSourcePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewAudioSource();
        }
    }

    private AudioSource CreateNewAudioSource()
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        ConfigureAudioSource(audioSource);
        audioSourcePool.Add(audioSource);
        return audioSource;
    }

    private void ConfigureAudioSource(AudioSource audioSource)
    {
        audioSource.spatialBlend = 1f;       // Set to 3D sound
        audioSource.playOnAwake = false;     // Do not play on awake
        audioSource.dopplerLevel = dopplerLevel;
        audioSource.spread = spread;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
    }

    private void PlayRandomSound()
    {
        // Fetch a random sound effect from the AudioManager
        AudioClip clip = AudioManager.Instance.GetRandomClipFromGroup(audioGroup);

        if (clip != null)
        {
            AudioSource audioSource = GetAvailableAudioSource();
            if (audioSource != null)
            {
                audioSource.clip = clip;

                // Randomize volume and pitch within specified ranges
                audioSource.volume = Random.Range(volumeRange.x, volumeRange.y);
                audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);

                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("No available AudioSources to play the sound.");
            }
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        // Find an inactive AudioSource
        foreach (AudioSource source in audioSourcePool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        // If all AudioSources are playing and we haven't reached max pool size, create a new one
        if (audioSourcePool.Count < maxPoolSize)
        {
            return CreateNewAudioSource();
        }

        // All AudioSources are busy, and we've reached max pool size
        return null;
    }
}
