using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    private int playerVisionLayermask;
    private GameObject player;
    [SerializeField] private Transform cameraLocation;

    private bool alerted = false;
    [Range(0,1)] private float alertMeter = 0;
    [SerializeField] private float alertIncreaseSpeed;

    private void Awake() 
    {
        int layermask1 = 1 << 9;
        int layermask2 = 1 << 6;
        int layermask3 = 1 << 0;
        playerVisionLayermask = layermask1 | layermask2 | layermask3;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update() 
    {
        if (CanSeePlayer())
        {
            alertMeter = Mathf.Min(alertMeter + alertIncreaseSpeed * Time.deltaTime, 1f);
            if ((alertMeter == 1) && !alerted)
            {
                alerted = true;
            }
        }
        else
        {
            alertMeter = Mathf.Max(alertMeter - alertIncreaseSpeed * 0.5f * Time.deltaTime, 0f);
            if ((alertMeter == 0) && alerted)
            {
                alerted = false;
            }
        }
    }

    private bool CanSeePlayer()
    {
        if (!player.activeInHierarchy) {return false;}

        RaycastHit hit;
        Vector3 playerDir = ((player.transform.position + new Vector3(0, 0.9f, 0)) - cameraLocation.position).normalized; //adding 0.9f to compensate for height
        if(Physics.Raycast(cameraLocation.position, playerDir, out hit, 20f, playerVisionLayermask))
        {
            if (hit.collider.gameObject.layer == 9)
            {
                return true;
            }
        }

        return false;
    }
}
