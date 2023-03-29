using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCooldowns : MonoBehaviour
{
    public static AbilityCooldowns Instance;

    [SerializeField] private float disableCooldown;
    private float disableCooldownTimer = 0f;

    private void Awake() 
    {
        Instance = this;
    }

    private void Update() 
    {
        if (disableCooldownTimer > 0f)
        {
            disableCooldownTimer -= Time.deltaTime;
        }   
    }

    public bool CanDisable()
    {
        return disableCooldownTimer <= 0f;
    }

    public void SetDisableCooldown()
    {
        disableCooldownTimer = disableCooldown;
    }
}
