using UnityEngine;
using UnityEngine.Playables;

public class WinGameButton : MonoBehaviour
{
    public Animator buttonAnimator;  // Assign the button's animator
    public PlayableDirector cutsceneDirector;
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
