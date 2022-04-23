using UnityEngine;
using System;
using System.Collections.Generic;

public class BattleManager: MonoBehaviour
{
    public static BattleManager self;
    public int friendsAmount = 5;
    public int friendsAlive;
    public int enemiesAmount = 5;
    public int enemiesAlive;
    public Unit prefab;
    public Mover mover { get; private set; }
    public List<Unit> units;
    public GamePhase gamePhase;

    void Awake()
    {
        self = this;
        units = new List<Unit>();
        mover = Mover.Start;
        gamePhase = GamePhase.Playing;
        friendsAlive = friendsAmount;
        enemiesAlive = enemiesAmount;
        GenerateUnits();
    }

    void Update()
    {
        if (gamePhase != GamePhase.Playing)
        {
            return;
        }
        
        switch (mover)
        {
            case Mover.Start:
                mover = Mover.Player;
                Debug.Log("Start -> Player move");
                return;
        }

        if (enemiesAlive == 0)
        {
            Debug.Log("Player won");
            gamePhase = GamePhase.Win;
        }

        if (friendsAlive == 0)
        {
            Debug.Log("Player defeated");
            gamePhase = GamePhase.Defeat;
        }
    }

    public void StopPlayerMove()
    {
        if (mover != Mover.Player)
        {
            Debug.LogError("Not player move");
            return;
        }

        Debug.Log("Player -> Enemy move");
        mover = Mover.Enemy;
    }

    public void StopEnemyMove()
    {
        if (mover != Mover.Enemy)
        {
            Debug.LogError("Not enemy move");
            return;
        }
        Debug.Log("Enemy -> Player move");
        mover = Mover.Player;
    }

    void GenerateUnits()
    {
        for (int i = 0; i < friendsAmount; i++)
        {
            var info = new UnitInfo(false, -5, 1.5f * (i - (friendsAmount - 1) / 2f), 0);
            var obj = Instantiate(prefab);
            obj.Init(info);

            units.Add(obj);
        }
        for (int i = 0; i < enemiesAmount; i++)
        {
            var info = new UnitInfo(true, 5, 1.5f * (i - (enemiesAmount - 1) / 2f), 0);
            var obj = Instantiate(prefab);
            obj.Init(info);

            units.Add(obj);
        }
    }

    public void Fight(int attackIndex, int defendIndex)
    {
        var attacker = units[attackIndex];
        var defender = units[defendIndex];
        if (attacker == null || defender == null)
        {
            Debug.Log(units.Count);
            throw new Exception("Who is apsent???");
        }
        defender.Info.Health -= attacker.Info.Strength;
        if (defender.Info.IsDestroyed)
        {
            if (defender.Info.IsEnemy)
            {
                enemiesAlive--;
            }
            else
            {
                friendsAlive--;
            }

            Destroy(units[defendIndex].gameObject);
        }
    }
}

public enum Mover
{
    Enemy,
    Start,
    Player
}

public enum GamePhase
{
    Win,
    Playing,
    Defeat
}
