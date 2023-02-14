using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    private AIPath aIPath;

    [SerializeField] private List<Transform> patrolRoute = new List<Transform>();
    private int patrolIndex = 0;
    private bool reversingThroughPatrol = false;
    private bool waiting = false;
    private bool patrolling = true;
    [SerializeField] private float patrolWaitTime;

    private void Awake() 
    {
        aIPath = GetComponent<AIPath>();
        SetMovePosition(patrolRoute[patrolIndex].position);    
    }

    private void Update() 
    {
        if (patrolling)
        {
            CheckDestinationReached();
        }
    }

    public void CheckDestinationReached()
    {
        if (aIPath.reachedDestination && !waiting)
        {
            StartCoroutine(UpdatePatrol());
        }
    }

    private IEnumerator UpdatePatrol()
    {
        waiting = true;

        yield return new WaitForSeconds(patrolWaitTime);

        if (patrolIndex == (patrolRoute.Count - 1))
        {
            reversingThroughPatrol = true;
        }
        else if (patrolIndex == 0)
        {
            reversingThroughPatrol = false;
        }

        if (reversingThroughPatrol)
        {
            patrolIndex--;
        }
        else
        {
            patrolIndex++;
        }
        SetMovePosition(patrolRoute[patrolIndex].position);

        waiting = false;
    }

    public void ChasePlayer(Vector3 playerTransform)
    {
        patrolling = false;
        SetMovePosition(playerTransform);
    }

    private void SetMovePosition(Vector3 movePosition)
    {
        aIPath.destination = movePosition;
    }
}
