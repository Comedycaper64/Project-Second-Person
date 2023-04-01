using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int health;

    private void Awake() 
    {
        health = maxHealth;    
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Camera") && other.TryGetComponent<EnemyMovement>(out EnemyMovement movement) && !movement.patrolling)
        {
            health--;
            movement.StunEnemy();
            if (health <= 0)
            {
                Debug.Log("You are dead");
            }
        }    
    }
}
