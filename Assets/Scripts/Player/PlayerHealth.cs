using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int health;
    public event Action OnDeath;

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
                OnDeath?.Invoke();
                UIManager.Instance.ToggleDeathUI(true);
            }
        }    
    }
}
