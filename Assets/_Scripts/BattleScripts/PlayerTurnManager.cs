using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerTurnManager : MonoBehaviour
{
    public static PlayerTurnManager self;
    public int selectedUnit;
    public int selectedEnemy;
    public Selected selected;
    private int amount;
    private BattleManager bm;
    private bool[] used;

    public int SelectedIndex { get; private set; }

    private void Awake()
    {
        self = this;
        bm = BattleManager.self;
        amount = bm.units.Count;
        used = new bool[amount];
        selectedUnit = -1;
        selectedEnemy = -1;
        selected = Selected.Nodody;
    }

    private void Start()
    {
        StartCoroutine(MousePositionChecker());
    }

    private void Update()
    {
        if (bm.turn != Turn.Player || bm.gamePhase != GamePhase.Playing) return;

        if (Input.GetMouseButtonDown(0)) SelectUnitViaMouseClick();
        
        var index = 0;
        while (index < bm.playerUnitsAmount && (used[index] || bm.units[index].Info.IsDestroyed)) index++;
        if (index == bm.playerUnitsAmount)
        {
            for (var i = 0; i < bm.playerUnitsAmount; i++)
                used[i] = false;

            bm.StopPlayerMove();
        }
    }

    private void SelectUnitViaMouseClick()
    {
        if (bm.turn != Turn.Player) return;
        if (selected == Selected.Nodody)
        {
            if (SelectedIndex != -1 && !bm.units[SelectedIndex].Info.IsEnemysUnit)
            {
                selectedUnit = SelectedIndex;
                Debug.Log("Unit " + selectedUnit + " selected");
                selected = Selected.Unit;
            }
        }
        else if (selected == Selected.Unit)
        {
            if (SelectedIndex != -1 && bm.units[SelectedIndex].Info.IsEnemysUnit)
            {
                selectedEnemy = SelectedIndex;
                Debug.Log("Enemy " + (selectedEnemy - bm.playerUnitsAmount)  + " selected");
                selected = Selected.Enemy;
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
                SelectedIndex = index;
            else
                SelectedIndex = -1;
            yield return null;
        }
    }

    public void TryToAttack()
    {
        if (selectedEnemy != -1) StartCoroutine(Attack());
    }

    public void Deselect()
    {
        if (selected == Selected.Unit)
        {
            selected = Selected.Nodody;
            Debug.Log("Unit " + selectedUnit + " deselected");
            selectedUnit = -1;
        }
        if (selected == Selected.Enemy)
        {
            selected = Selected.Unit;
            Debug.Log("Enemy " +  (selectedEnemy - bm.playerUnitsAmount) + " deselected");
            selectedEnemy = -1;
        }
    }
    
    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.4f);
        Debug.Log("Unit " + selectedUnit + " attacked enemy " + (selectedEnemy - bm.playerUnitsAmount));
        bm.Fight(selectedUnit, selectedEnemy);
        used[selectedUnit] = true;
        selectedEnemy = -1;
        selectedUnit = -1;
        selected = Selected.Nodody;
        bm.turn = Turn.Player;
    }
}

public enum Selected
{
    Nodody,
    Unit,
    Enemy
}