using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSInputManager : MonoBehaviour
{
    [SerializeField] FPSMovement movement;
    [SerializeField] FPSLook look;
    [SerializeField] FPSInteract interact;
    [SerializeField] PauseMenu pause;
    
    FPSPlayerControls controls;
    FPSPlayerControls.GroundMovementActions groundMovement;

    Vector2 horizontalInput;
    Vector2 lookInput;
    InputDevice currentDevice;

    private void Awake()
    {
        // We create groundMovement to access the GroundMovementActions and so we dont have to write the full path to the action everytime
        controls = new FPSPlayerControls();
        groundMovement = controls.GroundMovement;

        // For horizontal movement, a Vector2 is needed to store the input from the player
        groundMovement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();

        // For jumping, we just need to know when the player pressed the jump button
        groundMovement.Jump.performed += _ => movement.OnJumpPressed();

        /* For looking, we split the input into two floats, one for the X axis and one for the Y axis,
           this is because we need to rotate the player and the camera separately,
           and doing it here allows us to control the sensitivity of each axis independently          */
        groundMovement.LookX.performed += ctx => lookInput.x = ctx.ReadValue<float>();
        groundMovement.LookY.performed += ctx => lookInput.y = ctx.ReadValue<float>();

        // Crouch input handling with a single callback
        groundMovement.Crouch.started += ctx => movement.OnCrouchPressed(true); // Start crouching
        groundMovement.Crouch.canceled += ctx => movement.OnCrouchPressed(false); // Stop crouching

        // Sprint input handling with a single callback
        groundMovement.Sprint.started += ctx => movement.OnSprintPressed(true); // Start sprinting
        groundMovement.Sprint.canceled += ctx => movement.OnSprintPressed(false); // Stop sprinting

        // Interact input handling with a single callback
        groundMovement.Interact.performed += ctx => interact.OnInteractPressed();

        // Escape key for pause menu
        groundMovement.Pause.performed += _ => pause.OnPausePressed();
    }


    private void Update()
    {
        // Movement & Look input are in update because we need to send the input to the respective classes every frame
        movement.RecieveInput(horizontalInput);
        look.RecieveInput(lookInput);    
        // Jump is not here because all we need to know is if the player pressed the jump button, which is handled in the Awake method where we subscribe to the event directly
    }

    private void OnEnable()
    {
        controls.Enable();

    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
