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
    private bool[] used;
    private int length;
    public int ChosenUnit { get; private set; }
    public int ChosenEnemy { get; private set; }
    bool IsAngry;

    void Awake()
    {
        self = this;
        bm = BattleManager.self;
        length = bm.units.Count;
        used = new bool[length];
        ChosenUnit = -1;
        ChosenEnemy = -1;
        IsAngry = false;
    }

    private bool IsGoodUnit(int index)
    {
        return 0 <= index && index < bm.friendsAmount && !used[index] && !bm.units[index].Info.destroyed;
    }

    private void UpdateChosenUnit(int direction)
    {
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

    private bool IsBadUnit(int index)
    {
        return bm.friendsAmount <= index && index < bm.friendsAmount + bm.enemiesAmount && !used[index] && !bm.units[index].Info.destroyed;
    }

    private void UpdateChosenEnemy(int direction)
    {
        var index = ChosenEnemy == -1 ? -1 : ChosenEnemy;
        var existNotPlayed = false;

        for (int i = 0; i < length; i++)
        {
            index = (length + index + direction) % length;
            if (IsBadUnit(index))
            {
                existNotPlayed = true;
                ChosenEnemy = index;
                break;
            }
        }

        if (!existNotPlayed)
        {
            ChosenEnemy = -1;
            for (int i = 0; i < length; i++)
                used[i] = false;

            bm.StopEnemyMove();
        }
    }


    void Update()
    {
        length = bm.units.Count;
        if (IsAngry)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                UpdateChosenEnemy(1);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                UpdateChosenEnemy(-1);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                UpdateChosenUnit(1);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                UpdateChosenUnit(-1);
            }
        }

        if (Input.GetKeyDown("space"))
        {
            if (IsAngry)
            {
                Debug.Log("Player is attacking enemy");
                used[ChosenEnemy] = true;
                bm.Fight(ChosenUnit, ChosenEnemy);
                IsAngry = false;
            }
            else
            {
                Debug.Log("Player is moving");
                used[ChosenUnit] = true;
                IsAngry = true;
            }
        }

        if (Input.GetKeyDown("escape") && IsAngry)
        {
            ChosenEnemy = -1;
            IsAngry = false;
            used[ChosenUnit] = false;
        }
    }
}
