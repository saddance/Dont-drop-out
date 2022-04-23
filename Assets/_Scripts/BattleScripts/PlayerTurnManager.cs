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
    private int amount;
    public int ChosenUnit { get; private set; }
    public int ChosenEnemy { get; private set; }
    bool isReady;

    void Awake()
    {
        self = this;
        bm = BattleManager.self;
        amount = bm.units.Count;
        used = new bool[amount];
        ChosenUnit = -1;
        ChosenEnemy = -1;
        isReady = false;
    }

    private bool IsGoodUnit(int index)
    {
        return 0 <= index && index < bm.playerUnitsAmount 
            && !used[index] && !bm.units[index].Info.IsDestroyed;
    }

    private void UpdateChosenUnit(int direction)
    {
        var index = ChosenUnit == -1 ? -1 : ChosenUnit;

        for (int i = 0; i < amount; i++)
        {
            index = (amount + index + direction) % amount;
            if (IsGoodUnit(index))
            {
                ChosenUnit = index;
                break;
            }
        }
    }

    private bool IsBadUnit(int index)
    {
        return bm.playerUnitsAmount <= index && index < bm.playerUnitsAmount + bm.enemyUnitsAmount 
            && !bm.units[index].Info.IsDestroyed;
    }

    private void UpdateChosenEnemy(int direction)
    {
        var index = ChosenEnemy == -1 ? -1 : ChosenEnemy;

        for (int i = 0; i < amount; i++)
        {
            index = (amount + index + direction) % amount;
            if (IsBadUnit(index))
            {
                ChosenEnemy = index;
                break;
            }
        }
    }


    void Update()
    {
        if (bm.Turn != Turn.Player || bm.gamePhase != GamePhase.Playing)
        {
            return;
        }
        
        amount = bm.units.Count;
        if (isReady)
        {
            if (ChosenEnemy == -1)
            {
                ChosenEnemy = bm.enemyUnitsAmount;
                while (ChosenEnemy < bm.playerUnitsAmount + bm.enemyUnitsAmount && bm.units[ChosenEnemy].Info.IsDestroyed)
                {
                    ChosenEnemy++;
                }
                if (ChosenEnemy == bm.playerUnitsAmount + bm.enemyUnitsAmount)
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
                while (ChosenUnit < bm.playerUnitsAmount && (used[ChosenUnit] || bm.units[ChosenUnit].Info.IsDestroyed))
                {
                    ChosenUnit++;
                }
                if(ChosenUnit == bm.playerUnitsAmount)
                {
                    ChosenUnit = -1;
                    for (int i = 0; i < bm.playerUnitsAmount; i++)
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
            if (isReady)
            {
                Debug.Log("Player is attacking enemy");
                bm.Fight(ChosenUnit, ChosenEnemy);
                used[ChosenUnit] = true;
                ChosenEnemy = -1;
                ChosenUnit = -1;
                isReady = false;
            }
            else
            {
                isReady = true;
            }
        }

        if (Input.GetKeyDown("escape") && isReady)
        {
            ChosenEnemy = -1;
            isReady = false;
            used[ChosenUnit] = false;
        }
    }
}
