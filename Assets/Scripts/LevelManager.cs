using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    private bool objectiveObtained = false;

    [SerializeField] private float levelTimer = 20f;
    private float currentTimer;
    private bool timerEnabled = false;
    public event Action OnTimerEnd;

    private void Awake() 
    {
        Instance = this;    
        SetObjectiveObtained(false);
        currentTimer = levelTimer;
    }

    private void Update() 
    {
        if (timerEnabled)
        {
            currentTimer -= Time.deltaTime;
            UIManager.Instance.UpdateTimerText(currentTimer);
            if (currentTimer <= 0)
            {
                timerEnabled = false;
                OnTimerEnd?.Invoke();
                UIManager.Instance.ToggleDeathUI(true);
            }
        }    
    }

    public void EnableTimer()
    {
        timerEnabled = true;
        UIManager.Instance.ToggleTimerUI(true);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int GetLevel()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetObjectiveObtained(bool obtain)
    {
        objectiveObtained = obtain;
    }

    public bool IsObjectiveObtained()
    {
        return objectiveObtained;
    }
}
