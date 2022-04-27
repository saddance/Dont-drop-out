using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnManager : MonoBehaviour
{
    public static EnemyTurnManager self;
    private BattleManager bm;
    private bool isMoving = false;

    void Awake()
    {
        self = this;
    }
    
    void Start()
    {
        bm = BattleManager.self;
    }


    void Update()
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
        if (!isMoving && bm.Turn == Turn.Enemy)
        {
            StartCoroutine(AttackOnPlayer());
        }
    }

    IEnumerator AttackOnPlayer()
    {
        isMoving = true;
        for (int i = bm.playerUnitsAmount; i < bm.playerUnitsAmount + bm.enemyUnitsAmount; i++)
        {
            if(bm.units[i] == null)
            {
                continue;
            }
            var playersUnitsAlive = new List<int>();
            for(int j = 0; j < bm.playerUnitsAmount; j++)
            {
                if (bm.units[j] != null)
                {
                    playersUnitsAlive.Add(j);
                }
            }
            if (playersUnitsAlive.Count == 0)
            {
                yield break;
            }
            var aim = playersUnitsAlive[Random.Range(0, playersUnitsAlive.Count - 1)];
            Debug.Log("Enemy " + (i - bm.playerUnitsAmount).ToString() + " attacked " + aim.ToString());
            bm.Fight(i, aim);
            playersUnitsAlive.Clear();
            yield return new WaitForSeconds(0.5f);
        }

        isMoving = false;
        bm.StopEnemyMove();
    }
}
