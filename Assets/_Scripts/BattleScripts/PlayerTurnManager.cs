using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerTurnManager : MonoBehaviour
{
    public static PlayerTurnManager self;
    public int chosenUnit;
    public int chosenEnemy;
    public Phase phase;
    private int amount;
    private BattleManager bm;
    private bool[] used;
    private GameObject button1;
    public int ChosenByMouseIndex { get; private set; }

    private void Awake()
    {
        self = this;
        bm = BattleManager.self;
        amount = bm.units.Count;
        used = new bool[amount];
        chosenUnit = -1;
        chosenEnemy = -1;
        phase = Phase.NododyChosen;
    }

    private void Start()
    {
        StartCoroutine(MousePositionChecker());
    }

    private void Update()
    {
        if (bm.turn != Turn.Player || bm.gamePhase != GamePhase.Playing) return;

        if (Input.GetMouseButtonDown(0)) ChooseUnitViaMouseClick();
        
        var index = 0;
        while (index < bm.playerUnitsAmount && (used[index] || bm.units[index].Info.IsDestroyed)) index++;
        if (index == bm.playerUnitsAmount)
        {
            for (var i = 0; i < bm.playerUnitsAmount; i++)
                used[i] = false;

            bm.StopPlayerMove();
        }
    }

    private void ChooseUnitViaMouseClick()
    {
        if (bm.turn != Turn.Player) return;
        if (phase == Phase.NododyChosen)
        {
            if (ChosenByMouseIndex != -1 && !bm.units[ChosenByMouseIndex].Info.IsEnemysUnit)
            {
                chosenUnit = ChosenByMouseIndex;
                phase = Phase.UnitChosen;
            }
        }
        else if (phase == Phase.UnitChosen)
        {
            if (ChosenByMouseIndex != -1 && bm.units[ChosenByMouseIndex].Info.IsEnemysUnit)
            {
                chosenEnemy = ChosenByMouseIndex;
                phase = Phase.EnemyChosen;
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

    public void TryToAttack()
    {
        if (chosenEnemy != -1) StartCoroutine(Attack());
    }

    public void CancelChoice()
    {
        if (phase == Phase.UnitChosen)
        {
            chosenUnit = -1;
            phase = Phase.NododyChosen;
        }
        if (phase == Phase.EnemyChosen)
        {
            chosenEnemy = -1;
            phase = Phase.UnitChosen;
        }
    }
    
    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.4f);
        Debug.Log("Player is attacking enemy");
        bm.Fight(chosenUnit, chosenEnemy);
        used[chosenUnit] = true;
        chosenEnemy = -1;
        chosenUnit = -1;
        phase = Phase.NododyChosen;
        bm.turn = Turn.Player;
    }
}

public enum Phase
{
    NododyChosen,
    UnitChosen,
    EnemyChosen
}