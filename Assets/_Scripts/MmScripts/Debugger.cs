using UnityEngine;

public class Debugger : MonoBehaviour
{
    public HeroMotion Hero;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.ExitGame();
        }

        if (Input.GetKeyDown(KeyCode.P)) GameManager.SaveGame();
    }
}