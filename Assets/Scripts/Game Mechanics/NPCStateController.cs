using UnityEngine;
using UnityEngine.AI;

public class NPCStateController : MonoBehaviour
{
    public enum ObjectState { Wandering, StandingStill, BeingHeld }
    public ObjectState currentState = ObjectState.StandingStill;

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // If being held, don't check the NavMeshAgent's state
        if (currentState == ObjectState.BeingHeld)
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

    private void SetState(ObjectState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            UpdateAnimations();
        }
    }

    private void UpdateAnimations()
    {
        switch (currentState)
        {
            case ObjectState.Wandering:
                animator.CrossFade("Walk", 0, 0);
                break;
            case ObjectState.StandingStill:
                animator.CrossFade("Idle_A", 0, 0);
                break;
            case ObjectState.BeingHeld:
                animator.CrossFade("Fly", 0, 0);
                break;
        }
    }
}
