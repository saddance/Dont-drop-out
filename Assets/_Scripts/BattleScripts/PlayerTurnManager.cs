using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnManager : MonoBehaviour
{
    public static PlayerTurnManager self;
    void Start()
    {
        self = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            BattleManager.self.StopPlayerMove();
        }
    }
}
