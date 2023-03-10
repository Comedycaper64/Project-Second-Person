using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class EnemyMovement : MonoBehaviour
{
    private AIPath aIPath;
    private InputReader inputReader;
    [SerializeField] private CameraObject cameraObject;
    [SerializeField] private List<Transform> patrolRoute = new List<Transform>();
    private int patrolIndex = 0;
    private bool reversingThroughPatrol = false;
    private bool waiting = false;
    public bool patrolling = true;
    private bool disabled = false;
    [SerializeField] private float patrolWaitTime;
    [SerializeField] private float disabledTime;
    private Transform playerTransform;

    private void Awake() 
    {
        aIPath = GetComponent<AIPath>();
        SetMovePosition(patrolRoute[patrolIndex].position);    
        inputReader = GameObject.FindGameObjectWithTag("Player").GetComponent<InputReader>();
        inputReader.DisableEvent += DisableEnemy;
    }

    private void Update() 
    {
        if (patrolling)
        {
            CheckDestinationReached();
        }
        else
        {
            SetMovePosition(playerTransform.position);
        }
    }

    public void CheckDestinationReached()
    {
        if (aIPath.reachedDestination && !waiting)
        {
            StartCoroutine(UpdatePatrol());
        }
    }

    private void DisableEnemy()
    {
        if (cameraObject.IsCameraActive() && !disabled && AbilityCooldowns.Instance.CanDisable())
        {
            StartCoroutine(DisableMovement());
        }
    }

    private IEnumerator DisableMovement()
    {
        AbilityCooldowns.Instance.SetDisableCooldown();
        disabled = true;
        ToggleMovement(false);
        yield return new WaitForSeconds(disabledTime);
        ToggleMovement(true);
        disabled = false;
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

    public void ResumePatrol()
    {
        patrolling = true;
        SetMovePosition(patrolRoute[patrolIndex].position);
    }

    public void ChasePlayer(Transform playerTransform)
    {
        patrolling = false;
        this.playerTransform = playerTransform;
    }

    public void SetMovePosition(Vector3 movePosition)
    {
        aIPath.destination = movePosition;
    }

    private void ToggleMovement(bool enable)
    {
        aIPath.canMove = enable;
    }
}
