using UnityEngine;
using System.Collections;
using System;

public class AmbientZone : MonoBehaviour
{
    public string ambientTrackGroupName = "Ambient";  // The group name for ambient sounds
    public string ambientTrackName;  // The specific sound in the group (e.g., "Beach", "Forest", etc.)

    private void Start()
    {
        // Check if the Main Camera is inside the ambient zone when the game starts
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        if (mainCamera != null)
        {
            Collider zoneCollider = GetComponent<Collider>();

            // Check if the Main Camera's position is within the trigger zone
            if (zoneCollider != null && zoneCollider.bounds.Contains(mainCamera.transform.position))
            {
                // Trigger ambient sound fade-in
                AudioManager.Instance.AmbientSFXFadeIn(ambientTrackGroupName, ambientTrackName);
            }
        }
    }

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
