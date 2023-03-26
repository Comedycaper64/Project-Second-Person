using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private InputReader inputReader;
    private CharacterController controller;
    [SerializeField] private Animator animator;
    [SerializeField] private float playerSpeed;

    private void Awake() 
    {
        inputReader = GetComponent<InputReader>();
        controller = GetComponent<CharacterController>();
    }

    private void Update() 
    {
        Move();    
    }

    private void Move()
    {
        Vector3 movement = new Vector3(inputReader.movementValue.x, 0, inputReader.movementValue.y);
        controller.Move(movement * playerSpeed * Time.deltaTime);
        FaceMovementDirection(movement);
        animator.SetFloat("Speed", movement.magnitude * playerSpeed * 5);
    }

    private void FaceMovementDirection(Vector3 movement)
    {
        if (movement == Vector3.zero) {return;}

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), 10f * Time.deltaTime);
    }
}
