using UnityEngine;
using UnityEngine.UI;

public class SensitivityAdjuster : MonoBehaviour
{
    public Slider sensitivitySlider;
    private GameObject mainCamera;
    private FPSLook fpsLookScript;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        fpsLookScript = mainCamera.GetComponent<FPSLook>();
        if (fpsLookScript == null)
            Debug.LogWarning("FPSLook script not found on the Main Camera");

        // Assign the slider if not set
        if (sensitivitySlider == null)
            sensitivitySlider = GetComponent<Slider>();

        // Load saved sensitivity or use the current value from the FPSLook script
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", fpsLookScript.GetSensitivity());
        fpsLookScript.SetSensitivity(savedSensitivity);
        sensitivitySlider.value = savedSensitivity;

        // Add a listener to call the method when the slider value changes
        sensitivitySlider.onValueChanged.AddListener(delegate { AdjustSensitivity(); });
    }

    public void AdjustSensitivity()
    {
        // Update the mouse sensitivity in the FPSLook script
        fpsLookScript.SetSensitivity(sensitivitySlider.value);
        // Save the sensitivity setting
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivitySlider.value);
    }
}
