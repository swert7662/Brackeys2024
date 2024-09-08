using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [Header("Footstep Settings")]
    [SerializeField] private AudioClip[] footstepClips; 
    [SerializeField] private float minPitch = 0.95f; 
    [SerializeField] private float maxPitch = 1.05f;
    [SerializeField] private float distanceBetweenSteps = 1.8f; // X-axis distance required for footstep sound
    [SerializeField] private float fallDistanceThreshold = 2.0f;// Y-axis distance required for landing sound

    private FPSMovement fpsMovement; // Reference to the FPSMovement script
    private Vector3 previousPosition;
    private float xDistanceTraveled; // Distance traveled in the XZ plane
    private float yDistanceTraveled; // Distance traveled along the Y axis
    private bool isFalling;


    private void Start()
    {
        // Get the FPSMovement component attached to the player
        fpsMovement = GetComponentInParent<FPSMovement>();
        if (footstepClips == null || footstepClips.Length == 0)
        {
            Debug.LogWarning("FootstepClips array is empty or not assigned in the PlayerFootsteps script.");
        }

        previousPosition = transform.position; 
    }

    private void Update()
    {
        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        // Calculate the distances traveled since the last frame
        Vector3 currentPosition = transform.position;
        float xzDistance = Vector3.Distance(new Vector3(previousPosition.x, 0, previousPosition.z), new Vector3(currentPosition.x, 0, currentPosition.z)); // XZ movement distance
        float yDistance = Mathf.Abs(previousPosition.y - currentPosition.y); // Y-axis movement distance

        xDistanceTraveled += xzDistance;
        yDistanceTraveled += yDistance;

        if (fpsMovement.IsGrounded())
        {
            if (isFalling && yDistanceTraveled >= fallDistanceThreshold)
            {
                // If the player was falling and reached the fall distance threshold, play two footstep sounds
                PlayFootstepSound();
                PlayFootstepSound();
                ResetDistances();
            }
            else if (xDistanceTraveled >= distanceBetweenSteps)
            {
                // If the player is grounded and traveled enough distance on the XZ plane, play a single footstep sound
                PlayFootstepSound();
                ResetDistances();
            }            
            isFalling = false; // Reset falling state since player is grounded
        }
        else if (!isFalling && yDistanceTraveled > 0)
        {
            isFalling = true;
        }

        previousPosition = currentPosition; // Update the previous position for the next frame
    }

    private void PlayFootstepSound()
    {
        // Play a random footstep sound from the "Footsteps" group
        AudioManager.Instance.PlaySFX("Footsteps");
    }

    private void ResetDistances()
    {
        xDistanceTraveled = 0f;
        yDistanceTraveled = 0f;
    }
}
