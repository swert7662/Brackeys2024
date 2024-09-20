using System;
using UnityEngine;
using UnityEngine.AI;

public class NPCStateController : MonoBehaviour
{
    public enum ObjectState { Wandering, StandingStill, BeingHeld, InZone, InAir }
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
        if (currentState == ObjectState.BeingHeld || currentState == ObjectState.InAir || currentState == ObjectState.InZone)
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
            SetState(ObjectState.InAir);
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
        //else
        //{
        //    Debug.Log(name + " has left the zone somehow...");
        //}
    }

    public void SetInAir(bool isInAir)
    {
        if (isInAir)
        {
            SetState(ObjectState.InAir);
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

            // Handle movement components based on the new state
            if (currentState == ObjectState.BeingHeld || currentState == ObjectState.InZone || currentState == ObjectState.InAir)
            {
                ManageMovementComponents(false);  // Disable NavMesh and movement components
            }
            else if (priorState == ObjectState.InAir && currentState == ObjectState.StandingStill)
            {
                Debug.Log(name + " has landed on the ground");
                ManageMovementComponents(true);   // Re-enable components when landing
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
            case ObjectState.InAir:
                animator.CrossFade("Roll", 0, 0);
                animator.CrossFade("Eyes_Trauma", 0, 1);
                break;


        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentState == ObjectState.InAir)
        {
            // Check if the point of collision is on a valid NavMesh surface
            NavMeshHit hit;
            bool isOnNavMesh = NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas);

            if (isOnNavMesh)
            {
                // If the object landed on a walkable surface, switch to the standing state and activate NavMesh
                SetInAir(false);
                Debug.Log(name + " has landed on a walkable surface");
            }
            else
            {
                // Keep the object in ragdoll-like state (InAir or another state) if it's not on a walkable surface
                Debug.Log(name + " has collided with a non-walkable surface");
                // You can keep it in "InAir" or create a ragdoll state if you want more control
            }
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
