using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using checkPointsManager.runtime;

public class EnemyPatrol : MonoBehaviour
{

    [Header("Patrol Mode")]
    public PatrolMode patrolMode;
    public enum PatrolMode
    {
        FOV,
        Collider,
        Spotlight
    }

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
    [SerializeField] private PickableItem pickableItem;    
    [SerializeField] private PushPullObject pushPullObject;

    [Header("FOV Settings")]
    public float viewRadius1 = 10f;
    [Range(0, 360)] public float viewAngle1 = 120f;
    public float viewRadius2 = 5f;
    [Range(0, 360)] public float viewAngle2 = 60f;

    [Header("Spotlight Settings")]
    [SerializeField] private int raysToCast = 100;
    public Light spotlight;
    public float spotlightRayDistance = 10f;
    public float rotationInterval = 10f;
    public float[] rotationAngles;
    private int currentAngleIndex = 0;

    private Vector3 facingDirection = Vector3.forward;

    private void Start()
    {
        ai = GetComponent<NavMeshAgent>();
        posInt = 0;
        ai.speed = normalSpeed;
        StartCoroutine(GoPosPoint());
        StartCoroutine(RotateEnemy());
    }

    private void Update()
    {
        dest = posPoints[posInt].position;
        playerDest = playerPoint.position;

        if (ai.velocity.magnitude > 0.1f)
        {
            facingDirection = ai.velocity.normalized;
        }

        switch (patrolMode)
        {
            case PatrolMode.FOV:
                CheckFOV();
                break;
            case PatrolMode.Collider:
                break;
            case PatrolMode.Spotlight:
                CheckSpotlight();
                break;
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
            posInt = (posInt + 1) % posPoints.Length;
        }
    }

    private IEnumerator RotateEnemy()
    {
        while (true)
        {
            if (rotationAngles.Length > 0)
            {
                float angle = rotationAngles[currentAngleIndex];
                Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

                float elapsedTime = 0f;
                float rotationDuration = 1f;

                while (elapsedTime < rotationDuration)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsedTime / rotationDuration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.rotation = targetRotation;

                currentAngleIndex = (currentAngleIndex + 1) % rotationAngles.Length;
            }

            yield return new WaitForSeconds(rotationInterval);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (patrolMode == PatrolMode.Collider && other.CompareTag("Player"))
        {
            isChasing = true;
            ai.speed = chasingSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (patrolMode == PatrolMode.Collider && other.CompareTag("Player"))
        {
            isChasing = false;
            ai.speed = normalSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandlePlayerCollision(collision.gameObject);
        }
    }

    private void HandlePlayerCollision(GameObject player)
    {
        pickableItem = player.gameObject.GetComponentInChildren<PickableItem>();
        if (pickableItem != null)
        {
            if (pickableItem.heldItem != null)
            {
                pickableItem.DropItem();
            }
        }
        // var playerCheckpoint = player.GetComponent<Player_Checkpoint>();
        // if (playerCheckpoint != null)
        // {
        //     playerCheckpoint.teleportToCheckpoint(playerCheckpoint.currentCheckpoint);
        // }
        FadingUI.instance.textDesc.text = "you got caught";
        StartCoroutine(FadingUI.instance.Kill());
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

    private void CheckSpotlight()
    {
        if (spotlight == null || spotlight.type != LightType.Spot) return;

        Vector3 spotlightDirection = spotlight.transform.forward;
        Vector3 spotlightPosition = spotlight.transform.position;
        float halfSpotAngle = spotlight.spotAngle / 2f;

        for (int i = 0; i < raysToCast; i++)
        {
            float angle = (i / (float)(raysToCast - 1)) * spotlight.spotAngle - halfSpotAngle;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * spotlightDirection;

            if (Physics.Raycast(spotlightPosition, direction, out RaycastHit hit, spotlightRayDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Vector3 directionToPlayer = (hit.point - transform.position).normalized;
                    float distanceToPlayer = Vector3.Distance(transform.position, hit.point);

                    if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, LayerMask.GetMask("Obstacle")))
                    {
                        HandlePlayerCollision(hit.collider.gameObject);
                        isChasing = true;
                        ai.speed = chasingSpeed;
                        return;
                    }
                }
            }

            Debug.DrawRay(spotlightPosition, direction * spotlightRayDistance, Color.red);
        }

        isChasing = false;
        ai.speed = normalSpeed;
    }

    private void OnDrawGizmos()
    {
        if (patrolMode == PatrolMode.FOV)
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
