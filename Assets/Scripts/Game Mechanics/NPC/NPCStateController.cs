using System;
using UnityEngine;
using UnityEngine.AI;

public class NPCStateController : MonoBehaviour
{
    public enum ObjectState { Wandering, StandingStill, BeingHeld, InZone, InAir }
    public ObjectState currentState = ObjectState.StandingStill;
    public LayerMask finalLayer;
    public RenderTexture finalScreen;

    private NavMeshAgent navMeshAgent;
    private NPCWanderNavMesh wanderScript;
    private Animator animator;
    private EndGameMenu endGameMenu;

    [SerializeField] private Transform boatParent;  // Parent object containing seats
    [SerializeField] private float lerpSpeed = 2f;  // Speed of lerp to seat

    private bool isSeated = false;                  // Check if the NPC is already seated
    private Transform targetSeat;                   // The seat the NPC is lerping towards
    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        wanderScript = GetComponent<NPCWanderNavMesh>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        GameObject endGameMenuObject = GameObject.FindWithTag("MainCanvas");
        if (endGameMenuObject != null)
        {
            endGameMenu = endGameMenuObject.GetComponent<EndGameMenu>();
        }
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
        if (inZone && !isSeated)
        {
            Debug.Log(name + " has entered the zone");
            SetState(ObjectState.InZone);

            // Convert LayerMask to layer index
            int layerIndex = GetLayerIndex(finalLayer);
            Debug.Log("Setting layer to index: " + layerIndex);
            SetLayerRecursively(this.gameObject, layerIndex);

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;  
            FindAndMoveToSeat();  // Find and move the NPC to a seat on the boat
        }
    }

    private void SetLayerRecursively(GameObject obj, int finalLayer)
    {
        // Set the layer of the passed-in GameObject
        obj.layer = finalLayer;

        // Traverse and set the layer of all child objects
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, finalLayer);
        }
    }

    private int GetLayerIndex(LayerMask layerMask)
    {
        // Loop through each bit position and find the active layer index
        for (int i = 0; i < 32; i++)
        {
            if (layerMask == (1 << i))
            {
                return i;
            }
        }
        return -1; // Return -1 if the layerMask value is not valid
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

        // Find all child transforms with the "BoatSeat" tag
        foreach (Transform seat in boatParent)
        {
            // Ensure the seat is tagged "BoatSeat" and has no children (meaning it's free)
            if (seat.CompareTag("BoatSeat") && seat.childCount == 0)
            {
                endGameMenu.SetEndGameScreen(finalScreen);
                col.enabled = false;
                targetSeat = seat;  // Set this seat as the target
                transform.SetParent(seat);  // Make the NPC a child of the seat                
                isSeated = false;   // Ensure the NPC starts lerping to the seat
                break;
            }
        }
    }


    private void FixedUpdate()
    {
        if (currentState == ObjectState.InZone && targetSeat != null && !isSeated)
        {
            // Ensure the NPC is parented to the seat to move relative to it
            if (transform.parent != targetSeat)
            {
                transform.SetParent(targetSeat);  // Set the NPC's parent to the seat
            }

            // Move the NPC smoothly towards the seat's local position (Vector3.zero for local space)
            //transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, lerpSpeed * Time.fixedDeltaTime);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, lerpSpeed * Time.fixedDeltaTime);

            // Check if the NPC is close enough to the seat
            if (Vector3.Distance(transform.localPosition, Vector3.zero) < 0.5f)
            {
                transform.localPosition = Vector3.zero;  // Snap to the seat's local position
                transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
                rb.constraints = RigidbodyConstraints.FreezePosition;                
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                rb.interpolation = RigidbodyInterpolation.None;
                //Turn off the collider
                
                isSeated = true;                         // Mark as seated
                Debug.Log(name + " has reached the seat");
            }
        }
    }
}
