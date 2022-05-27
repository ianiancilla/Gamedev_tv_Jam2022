using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader: MonoBehaviour
{
    static int MENU_INDEX = 0;
    static int GAME_INDEX = 1;
    static int GAMEOVER_INDEX = 2;

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(MENU_INDEX);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(GAME_INDEX);
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene(GAMEOVER_INDEX);
    }
}
