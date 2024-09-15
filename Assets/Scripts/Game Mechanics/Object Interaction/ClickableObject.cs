using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public Animator buttonAnimator; 
    public string cutsceneName;      // Placeholder for the cutscene trigger

    // Call this method when the object is clicked
    public void OnPressed()
    {
        Debug.Log("Object Pressed!");
        buttonAnimator.SetTrigger("Press");

        AudioManager.Instance.PlaySFX("Click"); 

        StartCutscene();
    }

    private void StartCutscene()
    {
        // Placeholder for triggering a cutscene
        Debug.Log("Cutscene starting: " + cutsceneName);
        // You can add actual cutscene logic here once available
    }
}
