using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrambler : MonoBehaviour
{
    public bool scramblerEnabled = true;
    private bool playerInRange;
    public int scramblerRange;
    public GridPosition scramblerPosition;

    public static Action scramblerToggled;

    private void Start() 
    {
        scramblerPosition = LevelGrid.Instance.GetGridPosition(transform.position);    
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerInRange)
        {
            ToggleScrambler(!scramblerEnabled);
        }    
    }

    public void ToggleScrambler(bool enable)
    {
        scramblerEnabled = enable;
        scramblerToggled?.Invoke();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.GetComponent<PlayerMovement>())
        {
            playerInRange = true;
        }    
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.GetComponent<PlayerMovement>())
        {
            playerInRange = false;
        } 
    }
}
