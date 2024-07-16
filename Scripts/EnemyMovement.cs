using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform[] players; // Array of players
    public float detectionRadius = 10f;
    public Transform aggressiveArea;

    private NavMeshAgent agent;
    private int currentWaypointIndex;
    private bool isChasingPlayer;
    private bool isReversing;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentWaypointIndex = 0;
        SetNextWaypoint();
    }

    void Update()
    {
        if (IsPlayerInAggressiveArea())
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    bool IsPlayerInAggressiveArea()
    {
        if (aggressiveArea == null)
            return false;

        foreach (Transform playerTransform in players)
        {
            if (Vector3.Distance(aggressiveArea.position, playerTransform.position) < detectionRadius)
            {
                return true;
            }
        }

        return false;
    }

    void ChasePlayer()
    {
        Transform nearestPlayer = GetNearestPlayer();
        if (nearestPlayer != null)
        {
            agent.SetDestination(nearestPlayer.position);
            isChasingPlayer = true;
        }
    }

    Transform GetNearestPlayer()
    {
        Transform nearestPlayer = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Transform playerTransform in players)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPlayer = playerTransform;
            }
        }

        return nearestPlayer;
    }

    void Patrol()
    {
        if (isChasingPlayer)
        {
            SetNextWaypoint();
            isChasingPlayer = false;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            SetNextWaypoint();
        }
    }

    void SetNextWaypoint()
    {
        if (waypoints.Length == 0)
            return;

        agent.SetDestination(waypoints[currentWaypointIndex].position);

        if (isReversing)
        {
            currentWaypointIndex--;
            if (currentWaypointIndex < 0)
            {
                currentWaypointIndex = 1;
                isReversing = false;
            }
        }
        else
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = waypoints.Length - 2;
                isReversing = true;
            }
        }

        // Ensure the index is within bounds
        currentWaypointIndex = Mathf.Clamp(currentWaypointIndex, 0, waypoints.Length - 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            AvoidEnemy(other.transform);
        }
    }

    void AvoidEnemy(Transform enemy)
    {
        Vector3 directionToEnemy = transform.position - enemy.position;
        Vector3 newDestination = transform.position + directionToEnemy;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(newDestination, out hit, 1.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
