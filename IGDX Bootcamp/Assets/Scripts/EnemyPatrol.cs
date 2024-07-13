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
        if (other.CompareTag("Player"))
        {
            isChasing = true;
            ai.speed = chasingSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false;
            ai.speed = normalSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
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
}
