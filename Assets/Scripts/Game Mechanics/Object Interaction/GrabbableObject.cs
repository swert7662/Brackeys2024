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
    private NavMeshAgent navMeshAgent;
    private NPCWanderNavMesh wanderScript;
    private NPCStateController npcStateController;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
        wanderScript = GetComponent<NPCWanderNavMesh>(); // Get the wandering script
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
        npcStateController.SetBeingHeld(true);

        // Disable NavMeshAgent and wander script when picked up
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = false;
        }

        if (wanderScript != null)
        {
            wanderScript.enabled = false;
        }

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
        npcStateController.SetBeingHeld(false);

        // Re-enable NavMeshAgent and wander script when dropped
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = true;
        }

        if (wanderScript != null)
        {
            wanderScript.enabled = true;
        }

        // Re-enable all colliders when dropped
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }
    }
}
