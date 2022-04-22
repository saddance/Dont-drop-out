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
        return 0 <= index && index < bm.friendsAmount 
            && !used[index] && !bm.units[index].Info.IsDestroyed;
    }

    private void UpdateChosenUnit(int direction)
    {
        var index = ChosenUnit == -1 ? -1 : ChosenUnit;

        for (int i = 0; i < length; i++)
        {
            index = (length + index + direction) % length;
            if (IsGoodUnit(index))
            {
                ChosenUnit = index;
                break;
            }
        }
    }

    private bool IsBadUnit(int index)
    {
        return bm.friendsAmount <= index && index < bm.friendsAmount + bm.enemiesAmount 
            && !bm.units[index].Info.IsDestroyed;
    }

    private void UpdateChosenEnemy(int direction)
    {
        var index = ChosenEnemy == -1 ? -1 : ChosenEnemy;

        for (int i = 0; i < length; i++)
        {
            index = (length + index + direction) % length;
            if (IsBadUnit(index))
            {
                ChosenEnemy = index;
                break;
            }
        }
    }


    void Update()
    {
        length = bm.units.Count;
        if (IsAngry)
        {
            if (ChosenEnemy == -1)
            {
                ChosenEnemy = bm.enemiesAmount;
                while (ChosenEnemy < bm.friendsAmount + bm.enemiesAmount && bm.units[ChosenEnemy].Info.IsDestroyed)
                {
                    ChosenEnemy++;
                }
                if (ChosenEnemy == bm.friendsAmount + bm.enemiesAmount)
                {
                    Debug.LogAssertion("No enemies to fight!");
                }
            }

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
            if (ChosenUnit == -1)
            {
                ChosenUnit = 0;
                while (ChosenUnit < bm.friendsAmount && (used[ChosenUnit] || bm.units[ChosenUnit].Info.IsDestroyed))
                {
                    ChosenUnit++;
                }
                if(ChosenUnit == bm.friendsAmount)
                {
                    ChosenUnit = -1;
                    for (int i = 0; i < bm.friendsAmount; i++)
                        used[i] = false;

                    bm.StopPlayerMove();
                }
            }

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
                bm.Fight(ChosenUnit, ChosenEnemy);
                used[ChosenUnit] = true;
                ChosenEnemy = -1;
                ChosenUnit = -1;
                IsAngry = false;
            }
            else
            {
                Debug.Log("Player is moving");
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
