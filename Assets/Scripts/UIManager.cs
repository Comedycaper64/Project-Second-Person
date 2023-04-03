using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        enemyLogics = enemiesObject.GetComponentsInChildren<EnemyLogic>();
    }

    private void Update() 
    {
        bool canSeePlayerThisFrame = false;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetTeleportText(int availableTeleports)
    {
        teleportText.text = "Available Teleports: " + availableTeleports;
    }
    
}
