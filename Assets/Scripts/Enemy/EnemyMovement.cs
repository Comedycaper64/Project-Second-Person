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
    [SerializeField] private GameObject patrolObject;
    private Transform[] patrolRoute;
    private int patrolIndex = 0;
    private bool reversingThroughPatrol = false;
    private bool waiting = false;
    public bool patrolling = true;
    private bool disabled = false;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float patrolWaitTime;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float disabledTime;
    private Transform playerTransform;
    public Vector3 lastKnownPlayerLocation;
    public bool canSeePlayer;

    private void Awake() 
    {
        patrolObject.transform.SetParent(null);
        patrolRoute = patrolObject.GetComponentsInChildren<Transform>();
        aIPath = GetComponent<AIPath>();
        SetMovePosition(patrolRoute[patrolIndex].position);    
        inputReader = GameObject.FindGameObjectWithTag("Player").GetComponent<InputReader>();
        inputReader.DisableEvent += DisableEnemy;
    }

    private void Update() 
    {
        if (patrolling)
        {
            if (CheckDestinationReached())
            {
                StartCoroutine(UpdatePatrol());
            }
        }
        else
        {
            if (canSeePlayer)
            {
                SetMovePosition(playerTransform.position);
            }
            else
            {
                if (CheckDestinationReached())
                {
                    transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
                }
                else
                {
                    SetMovePosition(lastKnownPlayerLocation);
                }   
            }
        }
    }

    public bool CheckDestinationReached()
    {
        if (aIPath.reachedDestination && !waiting)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void StunEnemy()
    {
        if (!disabled)
        {
            StartCoroutine(DisableMovement());
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

        if (patrolIndex == (patrolRoute.Length - 1))
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
        aIPath.maxSpeed = patrolSpeed;
        SetMovePosition(patrolRoute[patrolIndex].position);
    }

    public void ChasePlayer(Transform playerTransform)
    {
        patrolling = false;
        aIPath.maxSpeed = chaseSpeed;
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
