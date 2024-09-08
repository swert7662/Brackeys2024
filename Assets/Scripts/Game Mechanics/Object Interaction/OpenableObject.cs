using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableObject : MonoBehaviour
{
    [SerializeField] private float maxOpenAngle = 90f; // Maximum angle the door will swing open
    [SerializeField] private float openLerpSpeed = 2f; // Speed at which the door swings open
    [SerializeField] private float closeLerpSpeed = 3f; // Speed at which the door swings open

    private bool isOpening = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private Quaternion convertedAngle;
    private float currentLerpSpeed;

    private void Start()
    {
        // Store the initial rotation of the door/object
        initialRotation = transform.rotation;
        convertedAngle = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + maxOpenAngle, transform.eulerAngles.z);
        targetRotation = initialRotation;
        currentLerpSpeed = openLerpSpeed;
    }

    private void FixedUpdate()
    {
        // If we are not open or closed, lerp the door to the target rotation
        if (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * currentLerpSpeed);
        }
    }

    // Public function to be called when the door should open
    public void Open()
    {
        isOpening = !isOpening;
        if (!isOpening)
        {
            targetRotation = initialRotation;
            currentLerpSpeed = closeLerpSpeed;
        }
        else
        {
            targetRotation = convertedAngle;
            currentLerpSpeed = openLerpSpeed;
        }
    }
}
