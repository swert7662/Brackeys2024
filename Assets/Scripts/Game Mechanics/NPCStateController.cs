using System;
using UnityEngine;
using UnityEngine.AI;

public class NPCStateController : MonoBehaviour
{
    public enum ObjectState { Wandering, StandingStill, BeingHeld, InZone }
    public ObjectState currentState = ObjectState.StandingStill;

    private NavMeshAgent navMeshAgent;
    private NPCWanderNavMesh wanderScript;
    private Animator animator;

    [SerializeField] private Transform boatParent;  // Parent object containing seats
    [SerializeField] private float lerpSpeed = 2f;  // Speed of lerp to seat

    private bool isSeated = false;                  // Check if the NPC is already seated
    private Transform targetSeat;                   // The seat the NPC is lerping towards
    private Rigidbody rb;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        wanderScript = GetComponent<NPCWanderNavMesh>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {        
        // If being held or in the zone, don't check the NavMeshAgent's state
        if (currentState == ObjectState.BeingHeld || currentState == ObjectState.InZone)
            return;

        // Check if the NPC is moving or standing still
        if (navMeshAgent != null && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            SetState(ObjectState.StandingStill);
        }
        else if (navMeshAgent != null)
        {
            SetState(ObjectState.Wandering);
        }
    }

    private void MakeNoise()
    {
        throw new NotImplementedException();
    }

    // Call this method to set the state to BeingHeld from the GrabbableObject script
    public void SetBeingHeld(bool isHeld)
    {
        if (isHeld)
        {
            SetState(ObjectState.BeingHeld);
        }
        else
        {
            SetState(ObjectState.StandingStill);
        }
    }

    // Call this method to set the state to InZone when the NPC enters the specific zone
    public void SetInZone(bool inZone)
    {
        if (inZone)
        {
            SetState(ObjectState.InZone);
            FindAndMoveToSeat();  // Find and move the NPC to a seat on the boat
        }
        else
        {
            SetState(ObjectState.StandingStill);
        }
    }

    private void SetState(ObjectState newState)
    {
        ObjectState priorState = currentState;
        if (currentState != newState)
        {
            currentState = newState;
            UpdateAnimations();

            // Handle whether movement components should be enabled or disabled
            if (currentState == ObjectState.BeingHeld || currentState == ObjectState.InZone)
            {
                ManageMovementComponents(false);  // Disable movement components when held or in zone
            }
            else if (priorState == ObjectState.BeingHeld)
            {
                ManageMovementComponents(true);   // Enable movement components otherwise
            }
        }
    }

    private void ManageMovementComponents(bool enable)
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = enable;
        }

        if (wanderScript != null)
        {
            wanderScript.enabled = enable;
            if (enable)
            {
                // Set the new wander center when enabling the wander script
                wanderScript.SetWanderCenter(transform.position);
            }
        }
    }

    private void UpdateAnimations()
    {
        switch (currentState)
        {
            case ObjectState.Wandering:
                animator.CrossFade("Walk", 0, 0);
                animator.CrossFade("Eyes_Blink", 0, 1);
                break;
            case ObjectState.StandingStill:
                animator.CrossFade("Idle_A", 0, 0);
                animator.CrossFade("Eyes_Blink", 0, 1);
                break;
            case ObjectState.BeingHeld:
                animator.CrossFade("Fly", 0, 0);
                animator.CrossFade("Eyes_Spin", 0, 1);
                break;
            case ObjectState.InZone:
                animator.CrossFade("Spin", 0, 0);
                animator.CrossFade("Eyes_Excited", 0, 1);
                break;
        }
    }

    // Find a free seat and move the NPC to the seat
    private void FindAndMoveToSeat()
    {
        if (boatParent == null)
        {
            Debug.LogWarning("Boat parent not assigned for " + name);
            return;
        }

        rb.useGravity = false;  // Disable gravity to prevent the NPC from falling

        // Find all child transforms with the "BoatSeat" tag
        foreach (Transform seat in boatParent)
        {
            // Ensure the seat is tagged "BoatSeat" and has no children (meaning it's free)
            if (seat.CompareTag("BoatSeat") && seat.childCount == 0)
            {
                Debug.Log(name + " will sit on the seat: " + seat.name);
                rb.constraints = RigidbodyConstraints.FreezePosition;
                targetSeat = seat;  // Set this seat as the target
                transform.SetParent(seat);  // Make the NPC a child of the seat
                gameObject.layer = LayerMask.NameToLayer("Default");
                isSeated = false;   // Ensure the NPC starts lerping to the seat
                break;
            }
        }
    }


    private void FixedUpdate()
    {
        if (currentState == ObjectState.InZone && targetSeat != null && !isSeated)
        {
            // Lerp the NPC to the seat position
            transform.position = Vector3.Lerp(transform.position, targetSeat.position, lerpSpeed * Time.fixedDeltaTime);

            // Check if the NPC is close enough to the seat
            if (Vector3.Distance(transform.position, targetSeat.position) < 0.1f)
            {
                transform.position = targetSeat.position;  // Snap to the seat
                isSeated = true;                          // Mark as seated
            }
        }
    }
}
