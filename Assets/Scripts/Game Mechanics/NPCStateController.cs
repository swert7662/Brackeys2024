using UnityEngine;
using UnityEngine.AI;

public class NPCStateController : MonoBehaviour
{
    public enum ObjectState { Wandering, StandingStill, BeingHeld, InZone }
    public ObjectState currentState = ObjectState.StandingStill;

    private NavMeshAgent navMeshAgent;
    private NPCWanderNavMesh wanderScript;
    private Animator animator;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        wanderScript = GetComponent<NPCWanderNavMesh>();
        animator = GetComponent<Animator>();
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
        else
        {
            SetState(ObjectState.Wandering);
        }
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
        }
        else
        {
            SetState(ObjectState.StandingStill);
        }
    }

    private void SetState(ObjectState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            UpdateAnimations();

            // Handle whether movement components should be enabled or disabled
            if (currentState == ObjectState.BeingHeld || currentState == ObjectState.InZone)
            {
                ManageMovementComponents(false);  // Disable movement components when held or in zone
            }
            else
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
                animator.CrossFade("Spin", 0, 0); // Play Spin animation on layer 0
                animator.CrossFade("Eyes_Excited", 0, 1); // Play Eyes_Excited on layer 1
                break;
        }
    }
}
