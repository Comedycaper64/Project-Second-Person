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
                //Enable end of level UI
                LevelManager.Instance.LoadNextLevel();
            }
        }    
    }
}
