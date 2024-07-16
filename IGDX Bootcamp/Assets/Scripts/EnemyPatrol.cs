using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using checkPointsManager.runtime;

public class EnemyPatrol : MonoBehaviour
{
    private NavMeshAgent ai;
    [SerializeField] private float posTime;
    [SerializeField] private Transform[] posPoints;
    private int posInt;
    private Vector3 dest;

    public PatrolArea patrolArea;
    public Collider triggerCol;
    public bool isChasing;
    [SerializeField] private Transform playerPoint;
    private Vector3 playerDest;
    public float normalSpeed = 3.5f;
    public float chasingSpeed = 5.25f;
    public PickableItem pickableItem;

    [Header("FOV Settings")]
    public bool useFOV = true; // Set to true to use FOV, false to use collider

    public float viewRadius1 = 10f;
    [Range(0, 360)]
    public float viewAngle1 = 120f;

    public float viewRadius2 = 5f;
    [Range(0, 360)]
    public float viewAngle2 = 60f;

    private Vector3 facingDirection = Vector3.forward;

    private void Start()
    {
        ai = GetComponent<NavMeshAgent>();
        posInt = 0; // Start at the first patrol point
        ai.speed = normalSpeed;
        StartCoroutine(GoPosPoint());
    }

    private void Update()
    {
        dest = posPoints[posInt].position;
        playerDest = playerPoint.position;

        // Update facing direction based on the movement direction
        if (ai.velocity.magnitude > 0.1f)
        {
            facingDirection = ai.velocity.normalized;
        }

        if (useFOV)
        {
            CheckFOV();
        }

        if (patrolArea.isPlayerInArea && isChasing)
        {
            ai.SetDestination(playerDest);
        }
        else
        {
            ai.SetDestination(dest);
        }
    }

    private IEnumerator GoPosPoint()
    {
        while (true)
        {
            yield return new WaitForSeconds(posTime);
            posInt = (posInt + 1) % posPoints.Length; // Move to the next point and loop back to the start
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!useFOV && other.CompareTag("Player"))
        {
            isChasing = true;
            ai.speed = chasingSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!useFOV && other.CompareTag("Player"))
        {
            isChasing = false;
            ai.speed = normalSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (pickableItem.heldItem != null)
            {
                pickableItem.DropItem();
            }
            var playerCheckpoint = collision.gameObject.GetComponent<Player_Checkpoint>();
            if (playerCheckpoint != null)
            {
                playerCheckpoint.teleportToCheckpoint(playerCheckpoint.currentCheckpoint);
            }
        }
    }

    public void ChasePlayer()
    {
        ai.SetDestination(playerDest);
    }

    private void CheckFOV()
    {
        bool playerDetected = CheckSingleFOV(viewRadius1, viewAngle1) || CheckSingleFOV(viewRadius2, viewAngle2);
        isChasing = playerDetected;
        ai.speed = playerDetected ? chasingSpeed : normalSpeed;
    }

    private bool CheckSingleFOV(float radius, float angle)
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Player"));
        foreach (var target in targetsInViewRadius)
        {
            Transform targetTransform = target.transform;
            Vector3 directionToTarget = (targetTransform.position - transform.position).normalized;
            if (Vector3.Angle(facingDirection, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, LayerMask.GetMask("Obstacle")))
                {
                    Debug.DrawRay(transform.position, directionToTarget * distanceToTarget, Color.green);
                    return true;
                }
                else
                {
                    Debug.DrawRay(transform.position, directionToTarget * distanceToTarget, Color.red);
                }
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (useFOV)
        {
            DrawFOV(viewRadius1, viewAngle1, Color.yellow);
            DrawFOV(viewRadius2, viewAngle2, Color.blue);
        }
    }

    private void DrawFOV(float radius, float angle, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, radius);
        Vector3 viewAngleA = DirFromAngle(-angle / 2, false);
        Vector3 viewAngleB = DirFromAngle(angle / 2, false);

        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * radius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * radius);
    }

    private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += Mathf.Atan2(facingDirection.z, facingDirection.x) * Mathf.Rad2Deg;
        }
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }
}
