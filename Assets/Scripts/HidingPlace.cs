using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingPlace : MonoBehaviour
{
    private GameObject player;
    private bool playerHidden = false;
    private bool enemyInRange = false;

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
            UIManager.Instance.ToggleHideUI(true, true);
        }

        if (other.gameObject.TryGetComponent<EnemyLogic>(out EnemyLogic enemyLogic))
        {
            if (enemyLogic.alerted)
            {
                HidePlayer(false);
            }
            enemyInRange = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            player = null;
            UIManager.Instance.ToggleHideUI(false, true);
        }

        if (other.gameObject.TryGetComponent<EnemyLogic>(out EnemyLogic enemyLogic))
        {
            enemyInRange = false;
        }
    }

    private void HidePlayer(bool enable)
    {
        if ((player == null) || enemyInRange) {return;}

        player.SetActive(!enable);
        playerHidden = enable;
        if (enable)
        {
            UIManager.Instance.ToggleHideUI(true, false);
        }
        else
        {
            UIManager.Instance.ToggleHideUI(true, true);
        }
    }
}
