using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrabbableObject : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    private Collider[] colliders;     // Array to store all colliders on the object

    [SerializeField] private float followLerpSpeed = 10f;
    private NPCStateController npcStateController;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
        npcStateController = GetComponent<NPCStateController>(); // Get the NPCStateController
        colliders = GetComponentsInChildren<Collider>(); // Get all colliders (including child objects)
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            // Lerp the position of the object to the target position
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, followLerpSpeed * Time.fixedDeltaTime);
            objectRigidbody.MovePosition(newPosition);
        }
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
        if (npcStateController != null)
            npcStateController.SetBeingHeld(true);


        // Disable all colliders to avoid interaction with the player
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;

        if (npcStateController != null)
        {
            npcStateController.SetBeingHeld(false);
            npcStateController.SetInAir(true); // Set to the new InAir state
        }


        // Re-enable all colliders when dropped
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }
    }

    public void Throw(Vector3 throwDirection, float throwForce)
    {
        // Ensure the object is released from the grab point
        this.objectGrabPointTransform = null;

        // Re-enable gravity
        objectRigidbody.useGravity = true;
        objectRigidbody.isKinematic = false;  // Ensure that the rigidbody is not set to kinematic, so physics can affect it

        if (npcStateController != null)
        {
            npcStateController.SetBeingHeld(false);
            npcStateController.SetInAir(true); // Set to the new InAir state
        }

        // Re-enable all colliders when thrown
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }

        // Apply force to the rigidbody in the given direction
        objectRigidbody.AddForce(throwDirection.normalized * throwForce, ForceMode.Impulse);
        //objectRigidbody.velocity = throwDirection * throwForce * Time.deltaTime;
        Debug.Log("Throwing object: " + name);
    }

}
