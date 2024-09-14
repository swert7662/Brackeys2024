using UnityEngine;
using UnityEngine.AI;

public class NPCWanderNavMesh : MonoBehaviour
{
    public float wanderRadius = 10f;  // Radius within which the NPC can wander
    public float wanderTimer = 5f;    // Time interval for changing destination

    private NavMeshAgent agent;       // Reference to the NavMesh Agent
    private float timer;              // Timer to track time since last destination change

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();  // Get the NavMeshAgent component
        timer = wanderTimer;                   // Initialize the timer
    }

    void Update()
    {
        timer += Time.deltaTime;

        // When the timer reaches the wander time, pick a new destination
        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0f;
        }
    }

    // Function to find a random position within a defined radius
    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
}
