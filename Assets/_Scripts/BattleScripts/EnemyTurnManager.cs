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
        if (!isMoving && bm.mover == Mover.Enemy)
        {
            StartCoroutine(AttackOnPlayer());
        }
    }

    IEnumerator AttackOnPlayer()
    {
        isMoving = true;
        for (int i = bm.friendsAmount; i < bm.friendsAmount + bm.enemiesAmount; i++)
        {
            if(bm.units[i] == null)
            {
                continue;
            }
            var alive = new List<int>();
            for(int j = 0; j < bm.friendsAmount; j++)
            {
                if (bm.units[j] != null)
                {
                    alive.Add(j);
                }
            }
            if (alive.Count == 0)
            {
                yield break;
            }
            var aim = alive[Random.Range(0, alive.Count - 1)];
            Debug.Log("Enemy " + (i - bm.friendsAmount).ToString() + " attacked " + aim.ToString());
            bm.Fight(i, aim);
            alive.Clear();
            yield return new WaitForSeconds(0.5f);
        }

        isMoving = false;
        bm.StopEnemyMove();
    }
}
