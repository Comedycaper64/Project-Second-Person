using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //Set this as main menu if using as main menu manager
    [SerializeField] private GameObject currentOpenScreen;

    public void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }

    public void OpenScreen(GameObject screen)
    {
        if (currentOpenScreen)
        {
            currentOpenScreen.SetActive(false);
        }
        screen.SetActive(true);
        currentOpenScreen = screen;
    }

    public void CloseScreen(GameObject screen)
    {
        screen.SetActive(false);
        currentOpenScreen = null;
    }
}
