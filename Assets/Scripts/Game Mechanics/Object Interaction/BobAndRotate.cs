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

    private void Start()
    {
        // Store the initial position of the object
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Bobbing motion
        float newY = initialPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Rotating motion
        float xRotation = rotateAroundX ? speed * Time.deltaTime : 0f;
        float yRotation = rotateAroundY ? speed * Time.deltaTime : 0f;
        float zRotation = rotateAroundZ ? speed * Time.deltaTime : 0f;

        transform.Rotate(new Vector3(xRotation, yRotation, zRotation));
    }
}
