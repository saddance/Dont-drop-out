using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class BattleManager: MonoBehaviour
{

    [HideInInspector] public int playerUnitsAmount;
    [HideInInspector] public int enemyUnitsAmount;

    private int playerUnitsAlive;
    private int enemyUnitsAlive;
    
    public static BattleManager self;

    public Turn Turn;
    public Unit unitPrefab;
    
    public List<Unit> units;
    public GamePhase gamePhase;

    #region Start

    private void Awake()
    {
        self = this;
        units = new List<Unit>();
        Turn = Turn.Start;
        gamePhase = GamePhase.Playing;

        GenerateUnits();
    }

    private void GenerateUnits()
    {
        GenerateFriends();
        GenerateEnemies();
    }

    private void GenerateFriends()
    {
        var friends = GameManager.currentSave.personalities
            .Where(p => p.asFriend != null && p.asFriend.IsParticipating())
            .ToList();

        playerUnitsAlive = playerUnitsAmount = friends.Count;
        for (int i = 0; i < friends.Count; i++)
        {
            var info = new UnitInfo(friends[i].asFriend.self, false, new Vector3(-5, 1.5f * (i - (friends.Count - 1) / 2f)));
            InstantiateUnit(info);
        }
    }

    private void GenerateEnemies()
    {
        var enemies = GameManager.currentSave.personalities[GameManager.currentSave.battleWith].asEnemy.people.ToList();

        enemyUnitsAlive = enemyUnitsAmount = enemies.Count;
        for (int i = 0; i < enemies.Count; i++)
        {
            var info = new UnitInfo(enemies[i], true, new Vector3(5, 1.5f * (i - (enemies.Count - 1) / 2f)));
            InstantiateUnit(info);
        }
    }

    private void InstantiateUnit(UnitInfo info)
    {
        var obj = Instantiate(unitPrefab);
        obj.Init(info);
        units.Add(obj);
    }
    #endregion

    void Update()
    {
        if (gamePhase != GamePhase.Playing)
        {
            return;
        }
        
        switch (Turn)
        {
            case Turn.Start:
                Turn = Turn.Player;
                Debug.Log("Started. Now it's the Player's turn");
                return;
        }

        if (enemyUnitsAlive == 0)
        {
            Debug.Log("Player won");
            Turn = Turn.Nobody;
            gamePhase = GamePhase.Win;
            StartCoroutine(FinishBattle(true));
        }

        if (playerUnitsAlive == 0)
        {
            Debug.Log("Player lost");
            Turn = Turn.Nobody;
            gamePhase = GamePhase.Loss;
            StartCoroutine(FinishBattle(false));
        }
    }

    IEnumerator FinishBattle(bool isWin)
    {
        yield return new WaitForSeconds(1.5f);
        GameManager.EndBattle(isWin);
    }

    public void StopPlayerMove()
    {
        if (Turn != Turn.Player)
        {
            Debug.LogError("It's not the Player's move");
            return;
        }

        Debug.Log("Player passed. Now it's the Enemy's turn");
        Turn = Turn.Enemy;
    }

    public void StopEnemyMove()
    {
        if (Turn != Turn.Enemy)
        {
            Debug.LogError("It's not the Enemy's move");
            return;
        }
        Debug.Log("Enemy passed. Now it's the Player's turn");
        Turn = Turn.Player;
    }

    public void Fight(int attackIndex, int defendIndex)
    {
        var attacker = units[attackIndex];
        var defender = units[defendIndex];
        if (attacker == null || defender == null)
        {
            Debug.Log(units.Count);
            throw new Exception("Who is absent?");
        }
        defender.Info.Health -= attacker.Info.Strength;
        if (defender.Info.IsDestroyed)
        {
            if (defender.Info.IsEnemysUnit)
            {
                enemyUnitsAlive--;
            }
            else
            {
                playerUnitsAlive--;
            }

            Destroy(units[defendIndex].gameObject);
        }
    }
}

public enum Turn
{
    Enemy,
    Start,
    Player,
    Nobody
}

public enum GamePhase
{
    Win,
    Playing,
    Loss
}
