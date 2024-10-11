using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections;

public class CameraShakeTrigger : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;
    public float shakeInterval = 30f;

    // Call this method to shake the camera
    [Button("Shake Camera")]  // Creates a button in the Inspector
    public void ShakeCamera()
    {
        AudioManager.Instance.PlaySFX("Earthquake");

        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
    }

    private void Start()
    {
        // Start the coroutine that triggers the camera shake every 30 seconds
        StartCoroutine(ShakeCameraCoroutine());
    }

    private IEnumerator ShakeCameraCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(shakeInterval);  // Wait for 30 seconds
            ShakeCamera();  // Call the ShakeCamera method to shake the camera
        }
    }
}
