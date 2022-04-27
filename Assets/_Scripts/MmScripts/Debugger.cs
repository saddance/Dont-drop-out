using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public HeroMotion Hero;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Hero.Pause)
                Hero.Pause = false;
            else
                GameManager.ExitGame();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.SaveGame();
        }
    }
}
