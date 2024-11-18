using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class FOVAdjuster : MonoBehaviour
{
    public Slider fovSlider;
    private GameObject mainCamera;
    private CinemachineVirtualCamera virtualCamera;

    void Awake()
    {
        // Find the Cinemachine Virtual Camera in the scene
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        virtualCamera = mainCamera.GetComponent<CinemachineVirtualCamera>();

        if (virtualCamera == null)
        {
            Debug.LogError("No Cinemachine Virtual Camera found in the scene.");
            return;
        }

        // Assign the slider if not set
        if (fovSlider == null)
            fovSlider = GetComponent<Slider>();

        // Load saved FOV or use the current virtual camera's FOV
        float savedFOV = PlayerPrefs.GetFloat("FOV", virtualCamera.m_Lens.FieldOfView);
        virtualCamera.m_Lens.FieldOfView = savedFOV;
        fovSlider.value = savedFOV;

        // Add a listener to call the method when the slider value changes
        fovSlider.onValueChanged.AddListener(delegate { AdjustFOV(); });
    }

    public void AdjustFOV()
    {
        // Update the virtual camera's field of view
        virtualCamera.m_Lens.FieldOfView = fovSlider.value;
        // Save the FOV setting
        PlayerPrefs.SetFloat("FOV", fovSlider.value);
    }
}
