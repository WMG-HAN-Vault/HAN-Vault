using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TeacherController : MonoBehaviour
{
    public Transform[] destinations;
    public float viewRadius = 10f;
    public float viewAngle = 90f;
    public LayerMask obstacleMask;

    private int currentGoalIndex = 0;
    private int direction = 1;
    private NavMeshAgent agent;
    private Transform player;
    private float chaseTimer = 0f;
    private float chaseDuration = 5f;
    private float chaseSpeed;
    private float chaseViewRadius;
    private float baseSpeed;
    private float baseViewRadius;

    private enum State { Patrol, Chase }
    private State currentState = State.Patrol;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        baseSpeed = agent.speed;
        baseViewRadius = viewRadius;
        chaseViewRadius = viewRadius * 1.5f;
        chaseSpeed = agent.speed * 1.3f;

        agent.SetDestination(destinations[currentGoalIndex].position);
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                if (CanSeeTarget() || CanHearPlayer())
                {
                    currentState = State.Chase;
                    chaseTimer = chaseDuration;
                }
                break;

            case State.Chase:
                Chase();
                break;
        }
    }

    void Patrol()
    {
        if (!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance &&
            (!agent.hasPath || agent.velocity.sqrMagnitude == 0f))
        {
            currentGoalIndex += direction;

            if (currentGoalIndex >= destinations.Length)
            {
                direction = -1;
                currentGoalIndex = destinations.Length - 2;
            }
            else if (currentGoalIndex < 0)
            {
                direction = 1;
                currentGoalIndex = 1;
            }

            agent.SetDestination(destinations[currentGoalIndex].position);
        }
    }

    void Chase()
    {
        agent.SetDestination(player.position);

        if (CanSeeTarget() || CanHearPlayer())
        {
            chaseTimer = chaseDuration;
            viewRadius = chaseViewRadius;
            agent.speed = chaseSpeed;
        }
        else
        {
            chaseTimer -= Time.deltaTime;

            if (chaseTimer <= 0f)
            {
                viewRadius = baseViewRadius;
                agent.speed = baseSpeed;
                currentState = State.Patrol;
                agent.SetDestination(destinations[currentGoalIndex].position);
            }
        }
    }

    public bool CanSeeTarget()
    {
        Transform target = player.transform; 

        Vector3 dirToTarget = (target.position - transform.position).normalized; 

        if (Vector3.Distance(transform.position, target.position) > viewRadius) 
            return false; 

        float angle = Vector3.Angle(transform.forward, dirToTarget); 

        if (angle > viewAngle / 2f) 
            return false; 

        if (Physics.Raycast(transform.position, dirToTarget, out RaycastHit hit, viewRadius, obstacleMask)) { 
            if (hit.transform != target) 
                return false; 
        }

        return true;
    }

    private bool CanHearPlayer()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();

        if (!playerController.IsMakingSound()) return false;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > viewRadius) return false;

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 left = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + left * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + right * viewRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player caught!");
        }
    }
}