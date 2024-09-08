using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FPSMovement : MonoBehaviour
{
    [SerializeField] LayerMask groundMask;
    [SerializeField] private float walkSpeed = 7f;
    [SerializeField] private float crouchMoveSpeed = 4f;
    [SerializeField] private float sprintSpeed = 11f;
    [SerializeField] float crouchHeight = 1.25f;
    [SerializeField] float jumpHeight = 3.5f;
    

    private Transform player;
    private Transform cameraTransform;
    private Transform crouchCameraPosition;
    private Transform standCameraPosition;
    private Transform groundCheckPosition;

    private CharacterController controller;
    private GroundCheck groundCheck;

    private bool jump;
    private bool isGrounded;
    private bool isCrouching = false;
    private bool isSprinting = false;
    private float gravity = -30f;
    private float speed;
    private float normalHeight;
    private Vector2 horizontalInput;
    Vector3 verticalVelocity = Vector3.zero;
    private float crouchTransitionSpeed = 10f;
    private float currentScale = 1f; 

   
    private void Awake()
    {
        player = this.transform;

        controller = GetComponent<CharacterController>();
        normalHeight = controller.height;

        // Get components by tags
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        crouchCameraPosition = GameObject.FindGameObjectWithTag("CrouchPosition").transform;
        standCameraPosition = GameObject.FindGameObjectWithTag("StandPosition").transform;

        GameObject groundCheckPositionGO = GameObject.FindGameObjectWithTag("GroundCheck");
        groundCheckPosition = groundCheckPositionGO.transform;
        groundCheck = groundCheckPositionGO.GetComponent<GroundCheck>();
    }
    private void Update()
    {
        isGrounded = groundCheck.IsGrounded();

        HandleHorizontalMovement();
        HandleJump();
        HandleCrouch();
        HandleVerticalMovement();
    }

    private bool CanStandUp()
    {
        float rayLength = normalHeight - crouchHeight;
        RaycastHit hit;
        if (Physics.Raycast(crouchCameraPosition.position, Vector3.up, out hit, rayLength, groundMask))
        {
            return false; 
        }
        return true;
    }


    private void HandleJump()
    {
        // If jump is true and the player is grounded, then jump
        if (jump) {
            if (isGrounded) {
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity * currentScale); // Jump eq -> v = sqrt(jumpHeight * -2 * gravity)
            }
            jump = false;
        }
    }

    private void HandleCrouch()
    {
        if (isCrouching)
        {
            // Instantly change the controller's height
            float previousHeight = controller.height;
            controller.height = crouchHeight;

            // Adjust player's vertical position to avoid sinking into the ground
            float heightDifference = previousHeight - crouchHeight;
            controller.center = new Vector3(controller.center.x, controller.center.y - heightDifference / 2, controller.center.z);

            // Smoothly transition the camera position to the crouch position
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, crouchCameraPosition.position, crouchTransitionSpeed * Time.deltaTime);
        }
        else if (!isCrouching && CanStandUp())
        {
            // Instantly change the controller's height
            float previousHeight = controller.height;
            controller.height = normalHeight;

            // Adjust player's vertical position to avoid sinking into the ground
            float heightDifference = normalHeight - previousHeight;
            controller.center = new Vector3(controller.center.x, controller.center.y + heightDifference / 2, controller.center.z);

            // Smoothly transition the camera position to the stand position
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, standCameraPosition.position, crouchTransitionSpeed * Time.deltaTime);
        }
    }

    private void HandleHorizontalMovement()
    {
        // Horizontal Movement section
        speed = walkSpeed;
        if (isSprinting)
        {
            speed = sprintSpeed;
        }
        if (isCrouching)
        {
            speed = crouchMoveSpeed;
        }
        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * speed;
        controller.Move(horizontalVelocity * Time.deltaTime);
    }

    private void HandleVerticalMovement()
    {   
        if (!isGrounded)
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }

        //verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }
    // Getters and Setters used to access private variables
    #region Getters and Setters
    public bool IsGrounded()
    {
        return isGrounded;
    }
    public bool IsCrouching()
    {
        return isCrouching;
    }

    public Vector3 GetVelocity()
    {
        return controller.velocity;
    }

    public float GetCurrentSpeed()
    {
        return speed;
    }

    public float GetWalkSpeed()
    {
        return walkSpeed;
    }

    public Transform GetCrouchCameraPosition()
    {
        return crouchCameraPosition;
    }

    public Transform GetStandCameraPosition()
    {
        return standCameraPosition;
    }
    public Transform GetGroundCheckPosition()
    {
        return groundCheckPosition;
    }

    public float GetCrouchHeight()
    {
        return crouchHeight;
    }

    public void SetCrouchHeight(float value)
    {
        crouchHeight = value;
    }

    public float GetNormalHeight()
    {
        return normalHeight;
    }

    public void SetNormalHeight(float value)
    {
        normalHeight = value;
    }

    #endregion

    // These methods are called from FPSInputManager.cs & handle input from the player
    #region Input Methods
    public void RecieveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput * currentScale;
    }

    public void OnJumpPressed()
    {
        jump = true;
    }

    public void OnCrouchPressed(bool toggle)
    {
        if (toggle)
        {
            isCrouching = true;
        }
        else if (!toggle)
        {
            isCrouching = false;
        }
    }

    public void OnSprintPressed(bool toggle)
    {
        if (toggle)
        {
            isSprinting = true;
        }
        else if (!toggle)
        {
            isSprinting = false;
        }
    }
    public void UpdatePlayerPosition(Vector3 newPosition)
    {
        controller.enabled = false;

        controller.transform.position = newPosition;

        controller.enabled = true;
    }

    internal void UpdateMovementParameters(float scaleAmount)
    {
        currentScale /= scaleAmount;

        // Adjust character controller height and radius based on the new scale
        controller.height /= scaleAmount;
        controller.radius /= scaleAmount;

        // Optionally adjust crouch and stand heights to match the new scale
        crouchHeight /= scaleAmount;
        normalHeight /= scaleAmount;

        crouchCameraPosition.localPosition /= scaleAmount;
        standCameraPosition.localPosition /= scaleAmount;
        groundCheckPosition.localPosition /= scaleAmount;

        // Adjust camera transition speed if needed
        //crouchTransitionSpeed *= scaleAmount;
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        if (controller != null)
        {
            Gizmos.color = Color.green;

            // Draw the base of the character controller (cylinder shape)
            Gizmos.DrawWireSphere(transform.position + controller.center + Vector3.up * (controller.height / 2 - controller.radius), controller.radius);
            Gizmos.DrawWireSphere(transform.position + controller.center - Vector3.up * (controller.height / 2 - controller.radius), controller.radius);
            Gizmos.DrawLine(
                transform.position + controller.center + Vector3.up * (controller.height / 2 - controller.radius) + Vector3.forward * controller.radius,
                transform.position + controller.center - Vector3.up * (controller.height / 2 - controller.radius) + Vector3.forward * controller.radius
            );
            Gizmos.DrawLine(
                transform.position + controller.center + Vector3.up * (controller.height / 2 - controller.radius) - Vector3.forward * controller.radius,
                transform.position + controller.center - Vector3.up * (controller.height / 2 - controller.radius) - Vector3.forward * controller.radius
            );
            Gizmos.DrawLine(
                transform.position + controller.center + Vector3.up * (controller.height / 2 - controller.radius) + Vector3.right * controller.radius,
                transform.position + controller.center - Vector3.up * (controller.height / 2 - controller.radius) + Vector3.right * controller.radius
            );
            Gizmos.DrawLine(
                transform.position + controller.center + Vector3.up * (controller.height / 2 - controller.radius) - Vector3.right * controller.radius,
                transform.position + controller.center - Vector3.up * (controller.height / 2 - controller.radius) - Vector3.right * controller.radius
            );
        }
    } 
    #endregion
}
