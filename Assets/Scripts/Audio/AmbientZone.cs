using UnityEngine;
using System.Collections;
using System;

public class AmbientZone : MonoBehaviour
{
    public string ambientTrackGroupName = "Ambient";  // The group name for ambient sounds
    public string ambientTrackName;  // The specific sound in the group (e.g., "Beach", "Forest", etc.)

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.AmbientSFXFadeIn(ambientTrackGroupName, ambientTrackName);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.AmbientSFXFadeOut(ambientTrackGroupName, ambientTrackName);
        }
    }
}
