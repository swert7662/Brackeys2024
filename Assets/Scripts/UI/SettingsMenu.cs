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

    [SerializeField] private Slider masterVolumeSlider;    
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider ambientVolumeSlider;
    //[SerializeField] private Toggle postProcessingToggle;

    void Start()
    {
        // Initialize settings with current values
        masterVolumeSlider.value = AudioManager.Instance.GetMasterVolume();
        musicVolumeSlider.value = AudioManager.Instance.GetMusicVolume();
        sfxVolumeSlider.value = AudioManager.Instance.GetSFXVolume();
        ambientVolumeSlider.value = AudioManager.Instance.GetAmbientVolume();
        //postProcessingToggle.isOn = mainCamera.GetUniversalAdditionalCameraData().renderPostProcessing;

        // Add listeners to handle changes
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        ambientVolumeSlider.onValueChanged.AddListener(SetAmbientVolume);
        //postProcessingToggle.onValueChanged.AddListener(TogglePostProcessing);
    }

    private void SetMasterVolume(float volume)
    {
        AudioManager.Instance.SetMasterVolume(volume);
    }

    private void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }

    private void SetSFXVolume(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);
    }

    private void SetAmbientVolume(float volume)
    {
        AudioManager.Instance.SetAmbientVolume(volume);
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
