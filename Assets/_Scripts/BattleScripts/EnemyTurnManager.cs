using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnManager : MonoBehaviour
{
    public static EnemyTurnManager self;
    private BattleManager bm;
    private bool isMoving;
    private PlayerTurnManager ptm;

    private void Awake()
    {
        self = this;
    }

    private void Start()
    {
        bm = BattleManager.self;
        ptm = PlayerTurnManager.self;
    }


    private void Update()
    {
        if (bm.gamePhase != GamePhase.Playing)
        {
            if (isMoving)
            {
                isMoving = false;
                StopCoroutine(AttackOnPlayer());
            }

            return;
        }

        if (!isMoving && bm.turn == Turn.Enemy) StartCoroutine(AttackOnPlayer());
    }

    private IEnumerator AttackOnPlayer()
    {
        isMoving = true;
        for (var i = bm.playerUnitsAmount; i < bm.playerUnitsAmount + bm.enemyUnitsAmount; i++)
        {
            yield return new WaitForSeconds(0.2f);
            if (bm.units[i] == null) continue;
            var playersUnitsAlive = new List<int>();
            for (var j = 0; j < bm.playerUnitsAmount; j++)
                if (bm.units[j] != null)
                    playersUnitsAlive.Add(j);
            if (playersUnitsAlive.Count == 0) yield break;
            var aim = playersUnitsAlive[Random.Range(0, playersUnitsAlive.Count)];
            Debug.Log("Enemy " + (i - bm.playerUnitsAmount) + " attacked " + aim);
            ptm.chosenUnit = aim;
            ptm.chosenEnemy = i;
            bm.Fight(i, aim);
            playersUnitsAlive.Clear();
            yield return new WaitForSeconds(0.5f);
        }

        isMoving = false;
        ptm.chosenUnit = -1;
        ptm.chosenEnemy = -1;
        if (bm.turn == Turn.Enemy)
            bm.StopEnemyMove();
    }
}