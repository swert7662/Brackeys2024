using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Cinemachine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject settingsMenuUI;

    //[SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineVirtualCamera mainCamera;

    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Toggle postProcessingToggle;

    void Start()
    {
        // Initialize settings with current values
        musicVolumeSlider.value = AudioManager.Instance.GetMusicVolume();
        sfxVolumeSlider.value = AudioManager.Instance.GetSFXVolume();
        //postProcessingToggle.isOn = mainCamera.GetUniversalAdditionalCameraData().renderPostProcessing;

        // Add listeners to handle changes
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        postProcessingToggle.onValueChanged.AddListener(TogglePostProcessing);
    }

    private void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }

    private void SetSFXVolume(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);
    }

    private void TogglePostProcessing(bool isEnabled)
    {
        Debug.Log("Do Nothing RN");
        //mainCamera.GetUniversalAdditionalCameraData().renderPostProcessing = isEnabled;
    }

    public void Back()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
    }
}
