using UnityEngine;

public class BobAndRotateWithEuler : MonoBehaviour
{
    [SerializeField] private float bobSpeed = 2f; // Speed of the bobbing motion
    [SerializeField] private float bobHeight = 0.05f; // Height of the bobbing motion
    [SerializeField] private float speed = 50f; // Overall rotation speed

    // Checkboxes to enable/disable rotation around each axis
    [SerializeField] private bool rotateAroundX = false;
    [SerializeField] private bool rotateAroundY = false;
    [SerializeField] private bool rotateAroundZ = false;

    private Vector3 initialPosition;
    private Vector3 initialRotation;

    private void Start()
    {
        // Store the initial position and rotation of the object
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles; // Save initial rotation to maintain Z rotation
    }

    private void Update()
    {
        // Bobbing motion
        float newY = initialPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Get current rotation as Euler angles
        Vector3 currentRotation = transform.eulerAngles;

        // Rotating motion, maintaining the initial Z rotation
        float xRotation = rotateAroundX ? speed * Time.deltaTime : 0f;
        float yRotation = rotateAroundY ? speed * Time.deltaTime : 0f;
        float zRotation = rotateAroundZ ? speed * Time.deltaTime : 0f;

        // Apply rotation only to Y axis while keeping the original Z rotation
        transform.eulerAngles = new Vector3(
            initialRotation.x + xRotation,
            currentRotation.y + yRotation,
            initialRotation.z // Keep initial Z rotation
        );
    }
}
