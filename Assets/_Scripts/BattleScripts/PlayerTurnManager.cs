using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    public int ChosenByMouseIndex { get; private set; }
    public bool isReady;

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

    void Start()
    {
        StartCoroutine(MousePositionChecker());
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

    private void ChooseUnitViaMouseClick()
    {
        if (bm.Turn != Turn.Player)
        {
            return;
        }
        if (!isReady)
        {
            if (ChosenByMouseIndex != -1 && !bm.units[ChosenByMouseIndex].Info.IsEnemysUnit)
            {
                ChosenUnit = ChosenByMouseIndex;
                Attack();
            }
        }
        else
        {
            if (ChosenByMouseIndex != -1 && bm.units[ChosenByMouseIndex].Info.IsEnemysUnit)
            {
                ChosenEnemy = ChosenByMouseIndex;
                Attack();
            }
        }
    }
    
    IEnumerator MousePositionChecker()
    {
        while(true)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int index = -1;
            double distance = double.MaxValue;
            for (int i = 0; i < bm.playerUnitsAmount + bm.enemyUnitsAmount; i++)
            {
                if (bm.units[i] == null || used[i])
                {
                    continue;
                }
                var unitPosition = bm.units[i].Info.Position;
                var distanceToUnit = Math.Sqrt((mousePos.x - unitPosition.x) * (mousePos.x - unitPosition.x) +
                                               (mousePos.y - unitPosition.y) * (mousePos.y - unitPosition.y));
                if (distanceToUnit < distance)
                {
                    distance = distanceToUnit;
                    index = i;
                }
            }
            
            if(index == -1)
            {
                ChosenByMouseIndex = -1;
                continue;
            }

            if (Math.Abs(mousePos.x - bm.units[index].Info.Position.x) < 0.5 && Math.Abs(mousePos.y - bm.units[index].Info.Position.y) < 0.5)
            {
                ChosenByMouseIndex = index;
            }
            else
            {
                ChosenByMouseIndex = -1;
            }
            yield return null;
        }
    }
    
    void Attack()
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


    void Update()
    {
        if (bm.Turn != Turn.Player || bm.gamePhase != GamePhase.Playing)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            ChooseUnitViaMouseClick();
        }
        
        amount = bm.units.Count;
        if (isReady)
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
            if (ChosenUnit == -1)
            {
                int index = 0;
                while (index < bm.playerUnitsAmount && (used[index] || bm.units[index].Info.IsDestroyed))
                {
                    index++;
                }
                if(index == bm.playerUnitsAmount)
                {
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
            Attack();
        }

        if (Input.GetKeyDown("escape") && isReady)
        {
            ChosenEnemy = -1;
            isReady = false;
            used[ChosenUnit] = false;
        }
    }
}
