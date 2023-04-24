using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractionPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if (other.GetComponent<PlayerMovement>())
        {
            if (LevelManager.Instance.IsObjectiveObtained())
            {
                if (LevelManager.Instance.GetLevel() == 5)
                {
                    UIManager.Instance.ToggleGameBeatUI();
                }
                else
                {
                    LevelManager.Instance.LoadNextLevel();
                }
            }
        }    
    }
}
