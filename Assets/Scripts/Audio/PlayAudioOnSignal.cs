using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayAudioOnSignal : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing on this GameObject.");
        }
    }

    public void SignalAudio()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
            Debug.Log("Signal received, playing audio.");
            //AudioManager.Instance.PlaySFX("Misc", "EngineRunning");
        }
    }
}
