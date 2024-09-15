using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public Animator buttonAnimator;
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
        Debug.Log("Cutscene starting: " + cutsceneName);
        // You can add actual cutscene logic here once available
    }
}
