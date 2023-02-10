using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Animator animator;
    [SerializeField] private float playerSpeed;

    private void Update() 
    {
        Move();    
    }

    private void Move()
    {
        Vector3 movement = new Vector3(inputReader.movementValue.x, 0, inputReader.movementValue.y);
        movement *= playerSpeed * Time.deltaTime;
        gameObject.transform.Translate(movement);
    }
}
