using System;
using System.Collections;
using UnityEngine;

public class PlayerTurnManager : MonoBehaviour
{
    public int chosenUnit;
    public int chosenEnemy;
    public bool isReady;
    private int amount;
    private BattleManager bm;
    private bool[] used;
    public int ChosenByMouseIndex { get; private set; }

    #region Start

    private void Awake()
    {
        bm = BattleManager.self;
        amount = bm.units.Count;
        used = new bool[amount];
        chosenUnit = -1;
        chosenEnemy = -1;
        isReady = false;
    }

    private void Start()
    {
        StartCoroutine(MousePositionChecker());
    }

    #endregion

    #region Update

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) ChooseUnitViaMouseClick();

        amount = bm.units.Count;
        if (isReady)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) UpdateChosenEnemy(1);

            if (Input.GetKeyDown(KeyCode.DownArrow)) UpdateChosenEnemy(-1);
        }
        else
        {
            if (chosenUnit == -1)
            {
                var index = 0;
                while (index < bm.playerUnitsAmount && (used[index] || bm.units[index].Info.IsDestroyed)) index++;
                if (index == bm.playerUnitsAmount)
                {
                    for (var i = 0; i < bm.playerUnitsAmount; i++)
                        used[i] = false;

                    bm.StopPlayerMove();
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow)) UpdateChosenUnit(1);

            if (Input.GetKeyDown(KeyCode.DownArrow)) UpdateChosenUnit(-1);
        }

        if (Input.GetKeyDown("space")) TryToAttack();

        if (Input.GetKeyDown("escape") && isReady)
        {
            chosenEnemy = -1;
            isReady = false;
            used[chosenUnit] = false;
        }
    }

    private bool IsGoodUnit(int index)
    {
        return 0 <= index && index < bm.playerUnitsAmount
                          && !used[index] && !bm.units[index].Info.IsDestroyed;
    }

    private void UpdateChosenUnit(int direction)
    {
        var index = chosenUnit == -1 ? -1 : chosenUnit;

        for (var i = 0; i < amount; i++)
        {
            index = (amount + index + direction) % amount;
            if (IsGoodUnit(index))
            {
                chosenUnit = index;
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
        var index = chosenEnemy == -1 ? -1 : chosenEnemy;

        for (var i = 0; i < amount; i++)
        {
            index = (amount + index + direction) % amount;
            if (IsBadUnit(index))
            {
                chosenEnemy = index;
                break;
            }
        }
    }

    private void ChooseUnitViaMouseClick()
    {
        if (bm.turn != Turn.Player) return;
        if (!isReady)
        {
            if (ChosenByMouseIndex != -1 && !bm.units[ChosenByMouseIndex].Info.IsEnemysUnit)
            {
                chosenUnit = ChosenByMouseIndex;
                TryToAttack();
            }
        }
        else
        {
            if (ChosenByMouseIndex != -1 && bm.units[ChosenByMouseIndex].Info.IsEnemysUnit)
            {
                chosenEnemy = ChosenByMouseIndex;
                TryToAttack();
            }
        }
    }

    private IEnumerator MousePositionChecker()
    {
        while (true)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var index = -1;
            var distance = double.MaxValue;
            for (var i = 0; i < bm.playerUnitsAmount + bm.enemyUnitsAmount; i++)
            {
                if (bm.units[i] == null || used[i]) continue;
                var unitPosition = bm.units[i].Info.Position;
                var distanceToUnit = Math.Sqrt((mousePos.x - unitPosition.x) * (mousePos.x - unitPosition.x) +
                                               (mousePos.y - unitPosition.y) * (mousePos.y - unitPosition.y));
                if (distanceToUnit < distance)
                {
                    distance = distanceToUnit;
                    index = i;
                }
            }

            if (index != -1 && Math.Abs(mousePos.x - bm.units[index].Info.Position.x) < 0.5 &&
                Math.Abs(mousePos.y - bm.units[index].Info.Position.y) < 0.5)
                ChosenByMouseIndex = index;
            else
                ChosenByMouseIndex = -1;
            yield return null;
        }
    }

    private void TryToAttack()
    {
        if (isReady)
        {
            if (chosenEnemy != -1) StartCoroutine(Attack());
        }
        else
        {
            if (chosenUnit != -1) isReady = true;
        }
    }

    private IEnumerator Attack()
    {
        bm.turn = Turn.Nobody;
        yield return new WaitForSeconds(0.4f);
        Debug.Log("Player is attacking enemy");
        bm.Fight(chosenUnit, chosenEnemy);
        used[chosenUnit] = true;
        chosenEnemy = -1;
        chosenUnit = -1;
        isReady = false;
        bm.turn = Turn.Player;
    }

    #endregion
}