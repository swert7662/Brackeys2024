using UnityEngine;

public class WinGameButton : MonoBehaviour
{
    public Animator buttonAnimator;  // Assign the button's animator
    public string cutsceneName;      // Placeholder for the cutscene trigger

    public void OnButtonPressed()
    {
        // Play the button push animation
        if (buttonAnimator != null)
        {
            buttonAnimator.SetTrigger("Pressed");
        }
        AudioManager.Instance.PlaySFX("Click");  // Play button press sound
        // Start the cutscene or trigger a win condition (placeholder)
        StartCutscene();
    }

    private void StartCutscene()
    {
        // Placeholder for triggering a cutscene
        Debug.Log("Cutscene starting: " + cutsceneName);
        // You can add actual cutscene logic here once available
    }
}
