using UnityEngine;
using UnityEngine.AI;

public class NPCWanderNavMesh : MonoBehaviour
{
    public float wanderRadius = 10f;  // Radius within which the NPC can wander
    public float wanderTimer = 5f;    // Time interval for changing destination
    private Vector3 wanderCenter;     // Fixed center point for wandering

    private NavMeshAgent agent;       // Reference to the NavMesh Agent
    private float timer;              // Timer to track time since last destination change
    private Vector3 targetPosition;   // To store the chosen destination

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();  // Get the NavMeshAgent component
        timer = wanderTimer;                   // Initialize the timer

        // Ensure we have a wander center
        if (wanderCenter == Vector3.zero)
        {
            wanderCenter = transform.position;  // Use initial position as the center
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        // When the timer reaches the wander time, pick a new destination
        if (timer >= wanderTimer)
        {
            targetPosition = RandomNavSphere(wanderCenter, wanderRadius, -1);
            agent.SetDestination(targetPosition);
            timer = 0f;
        }
    }

    // Function to set the wander center from an external script
    public void SetWanderCenter(Vector3 center)
    {
        wanderCenter = center;
    }

    // Function to find a random position within a defined radius from the wander center
    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    // Draw the wander area and target position in the Scene view
    private void OnDrawGizmos()
    {
        // Draw the wander radius around the fixed center
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(wanderCenter, wanderRadius);

        // Draw the target position
        if (targetPosition != Vector3.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(targetPosition, 0.5f);
        }
    }
}
