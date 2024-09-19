using UnityEngine;
using UnityEngine.Playables;

public class ClickableObject : MonoBehaviour
{
    public Animator buttonAnimator;
    public PlayableDirector cutsceneDirector;
    public string cutsceneName; // Placeholder for the cutscene trigger
    private SeatManager seatManager;

    private void Start()
    {
        seatManager = GetComponent<SeatManager>();
    }

    // Call this method when the object is clicked
    public void OnPressed()
    {
        Debug.Log("Object Pressed!");
        buttonAnimator.SetTrigger("Press");

        AudioManager.Instance.PlaySFX("Click");

        // Check for filled seats
        if (seatManager != null)
        {
            seatManager.OnSeatCheck();
        }

        StartCutscene();
    }

    private void StartCutscene()
    {
        // Play the Timeline cutscene via PlayableDirector
        if (cutsceneDirector != null)
        {
            cutsceneDirector.Play();  // Start playing the Timeline
            Debug.Log("Cutscene starting: " + cutsceneName);
        }
        else
        {
            Debug.LogError("No PlayableDirector assigned to start the cutscene.");
        }
    }
}
