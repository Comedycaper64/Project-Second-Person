using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private GameObject visibleText;
    [SerializeField] private GameObject alertedText;
    [SerializeField] private TextMeshProUGUI teleportText;
    [SerializeField] private GameObject deathUI;
    private GameObject enemiesObject;
    private EnemyLogic[] enemyLogics;

    private void Awake() 
    {
        Instance = this;    
        ToggleDeathUI(false);
        enemiesObject = GameObject.FindGameObjectWithTag("Enemies");
        if (enemiesObject)
        {
            enemyLogics = enemiesObject.GetComponentsInChildren<EnemyLogic>();
        }   
        else
        {
            enemyLogics = new EnemyLogic[0];
        }
    }

    private void Update() 
    {
        bool canSeePlayerThisFrame = false;
        if (enemyLogics.Length == 0) {return;}

        foreach(EnemyLogic logic in enemyLogics)
        {
            if(logic.GetCanSeePlayer())
            {
                canSeePlayerThisFrame = true;
            }
        }    
        ToggleVisibleUI(canSeePlayerThisFrame);
    }

    public void ToggleVisibleUI(bool enable)
    {
        visibleText.SetActive(enable);
    }

    public bool IsVisibleUIActive()
    {
        return visibleText.activeInHierarchy;
    }

    public void ToggleAlertedUI(bool enable)
    {
        alertedText.SetActive(enable);
    }

    public void ToggleDeathUI(bool enable)
    {
        deathUI.SetActive(enable);
    }
    
    public void ResetLevel()
    {
        LevelManager.Instance.ReloadLevel();
    }

    public void SetTeleportText(int availableTeleports)
    {
        teleportText.text = "Available Teleports: " + availableTeleports;
    }
    
}
