using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    private void Start()
    {
        ai = GetComponent<NavMeshAgent>();
        posInt = 0; // Start at the first patrol point
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
        if (other.gameObject.tag == "Player")
        {
            isChasing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            isChasing = false;
        }
    }

    public void ChasePlayer()
    {
        ai.SetDestination(playerDest);
    }
}
