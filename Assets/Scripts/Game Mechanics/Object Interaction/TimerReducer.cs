using UnityEngine;

public class TimerReducer : MonoBehaviour
{
    //[SerializeField] private TimerBar timerBar; 
    [SerializeField] private float timeReductionAmount = 5f; // Amount of time to reduce from the timer

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //timerBar.ReduceTimer(timeReductionAmount);

            PlayEatingSound();

            Destroy(gameObject);
        }
    }

    private void PlayEatingSound()
    {
        AudioManager.Instance.PlaySFX("Eating"); // REQUIRES AN AUDIOMANAGER INSTANCE
    }
}
