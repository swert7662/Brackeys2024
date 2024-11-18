using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class ClickableObject : MonoBehaviour
{
    public Animator buttonAnimator;
    public PlayableDirector cutsceneDirector;
    public string cutsceneName; // Placeholder for the cutscene trigger
    private SeatManager seatManager;
    public FPSMovement playerMovementComponent;

    private GameObject mainCamera;
    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        seatManager = GetComponent<SeatManager>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        virtualCamera = mainCamera.GetComponent<CinemachineVirtualCamera>();
    }

    // Call this method when the object is clicked
    public void OnPressed()
    {
        Debug.Log("Object Pressed!");
        buttonAnimator.SetTrigger("Press");

        AudioManager.Instance.PlaySFX("Misc", "EngineStart");

        // Check for filled seats
        if (seatManager != null)
        {
            seatManager.OnSeatCheck();
        }
        virtualCamera.Priority = 1;
        StartCutscene();
    }

    private void StartCutscene()
    {
        // Play the Timeline cutscene via PlayableDirector
        if (cutsceneDirector != null)
        {
            cutsceneDirector.Play();  // Start playing the Timeline
            playerMovementComponent.enabled = false;
            Debug.Log("Cutscene starting: " + cutsceneName);
        }
        else
        {
            Debug.LogError("No PlayableDirector assigned to start the cutscene.");
        }
    }
}
