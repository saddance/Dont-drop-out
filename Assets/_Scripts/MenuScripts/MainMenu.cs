using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        GameStateManager.NewGame();
    }

    public void QuitGame()
    {
        GameStateManager.ExitGame();
    }

    public void LoadGame() {
        GameStateManager.LoadGame();
    }
}
