using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnManager : MonoBehaviour
{
    private BattleManager bm;

    #region Update

    private IEnumerator AttackOnPlayer()
    {
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
            bm.ptm.chosenUnit = aim;
            bm.ptm.chosenEnemy = i;
            bm.Fight(i, aim);
            playersUnitsAlive.Clear();
            yield return new WaitForSeconds(0.5f);
        }

        bm.ptm.chosenUnit = -1;
        bm.ptm.chosenEnemy = -1;
        if (bm.turn == Turn.Enemy)
            bm.StopEnemyMove();
    }

    #endregion

    #region Start

    private void Awake()
    {
        bm = BattleManager.self;
    }

    private void Start()
    {
        StartCoroutine(AttackOnPlayer());
    }

    #endregion
}