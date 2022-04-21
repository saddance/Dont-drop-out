using UnityEngine;
using System;
using System.Collections.Generic;

public class BattleManager: MonoBehaviour
{
    public int friendsAmount = 5;
    public int enemiesAmount = 5;
    public static BattleManager self;
    public Unit prefab;
    public Mover mover { get; private set; }
    public List<Unit> units;
    System.Random rand;

    void Awake()
    {
        self = this;
        units = new List<Unit>();
        mover = Mover.Start;
        rand = new System.Random();
        GenerateUnits();
    }

    void Update()
    {
        switch (mover)
        {
            case Mover.Start:
                mover = Mover.Player;
                Debug.Log("Start -> Player move");
                return;
            case Mover.Enemy:
                StopEnemyMove();
                return;
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
            var info = new UnitInfo(false, -5, 1.5f * (i - (friendsAmount - 1) / 2f), 0, rand);
            var obj = Instantiate(prefab);
            obj.Init(info);

            units.Add(obj);
        }
        for (int i = 0; i < enemiesAmount; i++)
        {
            var info = new UnitInfo(true, 5, 1.5f * (i - (enemiesAmount - 1) / 2f), 0, rand);
            var obj = Instantiate(prefab);
            obj.Init(info);

            units.Add(obj);
        }
    }

    public void Fight(int firstIndex, int secondIndex)
    {
        var first = units[firstIndex];
        var second = units[secondIndex];
        if (first == null || second == null)
        {
            throw new Exception("Who is apsent???");
        }
        var firstStrength = first.Info.Strength;
        var secondStrength = second.Info.Strength;
        first.Info.Health -= secondStrength;
        second.Info.Health -= firstStrength;
    }
}

public enum Mover
{
    Enemy,
    Start,
    Player
}
