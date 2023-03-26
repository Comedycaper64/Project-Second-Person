using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private GameObject visibleText;
    [SerializeField] private GameObject alertedText;

    private void Awake() 
    {
        Instance = this;    
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
    
}
