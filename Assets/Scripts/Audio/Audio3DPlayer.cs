using UnityEngine;
using UnityEngine.Audio;

public class Audio3DPlayer: MonoBehaviour
{
    // Public variables
    private AudioSource audioSource; // The AudioSource used to play the 3D sounds
    public float interval = 5f;     // Interval between sound effects
    public string audioGroup;       // The name of the audio group to pull sounds from
    private float timer;            // Internal timer to track the interval

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                Debug.LogError("No AudioSource attached to " + gameObject.name);
            }
        }
        timer = interval; // Initialize the timer
    }

    private void Update()
    {
        if (audioSource == null) return;

        // Count down the timer
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // Play a new sound and reset the timer
            PlayRandomSound();
            timer = interval; // Reset the timer
        }
    }

    private void PlayRandomSound()
    {
        // Fetch a random sound effect from the AudioManager
        SoundEffect soundEffect = AudioManager.Instance.GetSFX(audioGroup);

        if (soundEffect != null)
        {
            // Assign the fetched sound clip to the AudioSource
            audioSource.clip = soundEffect.clip;
            audioSource.Play();
        }
    }
}
