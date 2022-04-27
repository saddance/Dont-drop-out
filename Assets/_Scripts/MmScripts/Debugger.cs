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
                GameStateManager.ExitGame();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameStateManager.SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            var saves = GameStateManager.GetSaveNames();
            print(string.Join(" ", saves));
        }
    }
}
