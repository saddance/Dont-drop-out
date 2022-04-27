using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        GameManager.StartNewGame();
    }

    public void QuitGame()
    {
        GameManager.ExitGame();
    }

    public void LoadGame()
    {
        GameManager.LoadSavedGame();
    }
}