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
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI hideText;
    [SerializeField] private TextMeshProUGUI disableText;
    [SerializeField] private GameObject deathUI;
    private GameObject enemiesObject;
    private EnemyLogic[] enemyLogics;
    [SerializeField] private AudioClip enemyAlertedSFX;
    [SerializeField] private AudioClip enemySeenSFX;

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
        if (enable && SoundManager.Instance && !IsVisibleUIActive())
        {
            AudioSource.PlayClipAtPoint(enemySeenSFX, Camera.main.transform.position, SoundManager.Instance.GetSoundEffectVolume());
        }
        visibleText.SetActive(enable);
    }

    public bool IsVisibleUIActive()
    {
        return visibleText.activeInHierarchy;
    }

    public void ToggleAlertedUI(bool enable)
    {
        alertedText.SetActive(enable);
        if (enable && SoundManager.Instance)
        {
            AudioSource.PlayClipAtPoint(enemyAlertedSFX, Camera.main.transform.position, SoundManager.Instance.GetSoundEffectVolume());
        }
    }

    public void ToggleDeathUI(bool enable)
    {
        deathUI.SetActive(enable);
    }

    public void ToggleTimerUI(bool enable)
    {
        timerText.gameObject.SetActive(enable);
    }

    public void ToggleHideUI(bool enable, bool hide)
    {
        if (hide)
        {
            hideText.text = "Hide [Spacebar]";
        }
        else
        {
            hideText.text = "Exit [Spacebar]";
        }
        hideText.gameObject.SetActive(enable);
    }

    public void ToggleInteractUI(bool enable)
    {
        disableText.gameObject.SetActive(enable);
    }

    public void UpdateTimerText(float newTime)
    {
        timerText.text = newTime.ToString("#.##");
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
