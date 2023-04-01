using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    private int playerVisionLayermask;
    private GameObject player;
    [SerializeField] private Transform cameraLocation;
    private EnemyMovement enemyMovement;
    public bool alerted = false;
    private UIManager uIManager;
    [Range(0,1)] private float alertMeter = 0;
    [SerializeField] private float alertIncreaseSpeed;
    [SerializeField] private float alertDecreaseSpeed;
    [Range(0,1)] [SerializeField] private float viewCone;

    private void Awake() 
    {
        int layermask1 = 1 << 9;
        int layermask2 = 1 << 6;
        int layermask3 = 1 << 0;
        playerVisionLayermask = layermask1 | layermask2 | layermask3;
        player = GameObject.FindGameObjectWithTag("Player");
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Start() 
    {
        uIManager = UIManager.Instance;    
    }

    private void Update() 
    {
        if (CanSeePlayer())
        {
            if (!uIManager.IsVisibleUIActive()) 
            {
                Debug.Log("ayaya");
                uIManager.ToggleVisibleUI(true);}

            alertMeter = Mathf.Min(alertMeter + alertIncreaseSpeed * Time.deltaTime, 1f);
            enemyMovement.lastKnownPlayerLocation = player.transform.position;
            enemyMovement.canSeePlayer = true;
            if ((alertMeter == 1) && !alerted)
            {
                alerted = true;
                uIManager.ToggleAlertedUI(true);
                enemyMovement.ChasePlayer(player.transform);
            }
        }
        else
        {
            if (uIManager.IsVisibleUIActive()) {uIManager.ToggleVisibleUI(false);}

            alertMeter = Mathf.Max(alertMeter - alertDecreaseSpeed * Time.deltaTime, 0f);
            enemyMovement.canSeePlayer = false;
            if ((alertMeter == 0) && alerted)
            {
                alerted = false;
                uIManager.ToggleAlertedUI(false);
                enemyMovement.ResumePatrol();
            }
        }
    }

    private bool CanSeePlayer()
    {
        if (!player.activeInHierarchy) {return false;}

        RaycastHit hit;
        Vector3 playerDir = ((player.transform.position + new Vector3(0, 0.9f, 0)) - cameraLocation.position).normalized; //adding 0.9f to compensate for height
        //Debug.Log(Vector3.Dot(cameraLocation.forward, playerDir));
        if (Vector3.Dot(cameraLocation.forward, playerDir) < (1 - viewCone))
        {
            return false;
        }

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
