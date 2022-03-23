using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string newGameLevel;

    public void startGameNewGame()
    {
        SceneManager.LoadScene("Level01");
    }

    public void startGameTestArea()
    {
        SceneManager.LoadScene("TestEnv");
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
