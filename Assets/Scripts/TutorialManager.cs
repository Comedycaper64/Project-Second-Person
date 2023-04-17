using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private int tutorialScreenIndex = 0;
    [SerializeField] private GameObject[] tutorialScreens;
    [SerializeField] private GameObject currentOpenScreen;
    [SerializeField] private GameObject background;

    public void OpenScreen(GameObject screen)
    {
        if (currentOpenScreen)
        {
            currentOpenScreen.SetActive(false);
        }
        screen.SetActive(true);
        currentOpenScreen = screen;
    }

    public void OpenTutorial()
    {
        OpenScreen(tutorialScreens[tutorialScreenIndex]);
        background.SetActive(true);
    }

    public void CloseTutorial()
    {
        tutorialScreens[tutorialScreenIndex].SetActive(false);
        currentOpenScreen = null;
        background.SetActive(false);
    }

    public void OpenNextTutorialScreen()
    {
        OpenScreen(tutorialScreens[tutorialScreenIndex + 1]);
        tutorialScreenIndex++;
    }

    public void OpenPreviousTutorialScreen()
    {
        OpenScreen(tutorialScreens[tutorialScreenIndex - 1]);
        tutorialScreenIndex--;
    }
}
