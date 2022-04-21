using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEditorInternal.VR;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerTurnManager : MonoBehaviour
{
    public static PlayerTurnManager self;
    private BattleManager bm;
    private List<Unit> units;
    private bool[] used;
    private int length;
    public int ChosenUnit { get; private set; }
    
    
    void Awake()
    {
        self = this;
        bm = BattleManager.self;
        units = bm.units;
        length = units.Count;
        used = new bool[length];
        ChosenUnit = -1;
        
    }

    private bool IsGoodUnit(int index)
    {
        return 0 <= index && index < length && !used[index] && !units[index].Info.IsEnemy;
    }
    private void UpdateChosenUnit(int direction = 0)
    {
        if (direction == 0)
        {
            if (IsGoodUnit(ChosenUnit))
                return;
            direction = 1;
        }

        var index = ChosenUnit == -1 ? -1 : ChosenUnit;
        var existNotPlayed = false;

        for (int i = 0; i < length; i++)
        {
            index = (length + index + direction) % length;
            if (IsGoodUnit(index))
            {
                existNotPlayed = true;
                ChosenUnit = index;
                break;
            }
        }

        if (!existNotPlayed)
        {
            ChosenUnit = -1;
            for (int i = 0; i < length; i++)
                used[i] = false;

            bm.StopPlayerMove();
        }
    }
   
    

    void Update()
    {
        if (ChosenUnit == -1)
            UpdateChosenUnit();

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            UpdateChosenUnit(1);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            UpdateChosenUnit(-1);
        }

        if (Input.GetKeyDown("space"))
        {
            Debug.Log("Player is moving");
            used[ChosenUnit] = true;
            UpdateChosenUnit();
        }
    }
}
