using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField] private AudioClip objectiveGetSFX;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.GetComponent<PlayerMovement>())
        {
            LevelManager.Instance.SetObjectiveObtained(true);
            LevelManager.Instance.EnableTimer();
            if (SoundManager.Instance)
            {
                AudioSource.PlayClipAtPoint(objectiveGetSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
            }
            Destroy(gameObject);
        }    
    }
}
