using UnityEngine;
using UnityEngine.InputSystem;
public class FPSLook : MonoBehaviour
{
    [SerializeField] private float sensitivity; // 30 is good for controller, 60 is good for mouse
    //[SerializeField] float gamepadMultiplier = 0.5f;

    private float lookX, lookY;
    private Transform playerHead;
    private Transform playerBody;
    private float xClamp = 85f;
    private float xRotation = 0f;

    private PlayerInput playerInput;


    private void Start()
    {
        // This script is attached to the camera, which is a child of the player body thus we can refer to it as the head, and grab the parent as the body
        playerHead = this.transform;
        playerBody = playerHead.parent;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Ensure the camera starts a 0, .75, 0 position and 0, 0, 0 rotation
        playerHead.position = new Vector3(0, 0.75f, 0);
        playerHead.localEulerAngles = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        HandleYRotation();
        HandleXRotation();
    }

    private void HandleXRotation()
    {
        // Pitch the camera around the X axis
        xRotation -= lookY * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        playerHead.eulerAngles = targetRotation;
    }


    private void HandleYRotation()
    {
        // For Y rotation we want to rotate the entire player body
        playerBody.Rotate(Vector3.up, lookX * Time.deltaTime);
    }

    public void RecieveInput(Vector2 lookInput)
    {
        lookX = lookInput.x * sensitivity;
        lookY = lookInput.y * sensitivity;
    }

    // Getters and Setters
    
    public float GetSensitivity() 
    {
        return sensitivity;
    }
    public void SetSensitivity(float sensitivity)
    {
        this.sensitivity = sensitivity;
    }
}