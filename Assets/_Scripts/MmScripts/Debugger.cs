using UnityEngine;

public class Debugger : MonoBehaviour
{
    public HeroMotion Hero;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Hero.Pause)
                Hero.Pause = false;
            else
                GameManager.ExitGame();
        }

        if (Input.GetKeyDown(KeyCode.P)) GameManager.SaveGame();
    }
}