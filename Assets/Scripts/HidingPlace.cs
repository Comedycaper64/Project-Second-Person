using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingPlace : MonoBehaviour
{
    private GameObject player;
    private bool playerHidden = false;

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HidePlayer(!playerHidden);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            player = null;
        }
    }

    private void HidePlayer(bool enable)
    {
        player.SetActive(!enable);
        playerHidden = enable;
    }
}
