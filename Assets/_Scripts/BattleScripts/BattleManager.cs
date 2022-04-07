using UnityEngine;
using System;
using System.Collections.Generic;

public class BattleManager: MonoBehaviour
{
    public static BattleManager self;
    public Unit prefab;
    public Mover mover { get; private set; }
    public List<Unit> units;
    private List<UnitInfo> unitsInfo;

    void Awake()
    {
        self = this;
        units = new List<Unit>();
        unitsInfo = new List<UnitInfo>();
        mover = Mover.Start;
        UnitsGenerate();
    }

    void Update()
    {
        if (mover == Mover.Start)
        {
            mover = Mover.Player;
            Debug.Log("Start -> Player move");
            return;
        }
        if (mover == Mover.Enemy)
        {
            mover = Mover.Player;
            Debug.Log("Enemy -> Player move");
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

    void UnitsGenerate()
    {
        for (int i = 0; i < 3; i++)
        {
            var info = new UnitInfo(false, -5, 2 * i, 0);
            var obj = Instantiate(prefab);
            obj.Init(info);

            units.Add(obj);
            unitsInfo.Add(info);
        }
        for (int i = 0; i < 3; i++)
        {
            var info = new UnitInfo(true, 5, 2 * i, 0);
            var obj = Instantiate(prefab);
            obj.Init(info);

            units.Add(obj);
            unitsInfo.Add(info);
        }
    }

    void Start()
    {
    }
}

public enum Mover
{
    Enemy = -1,
    Start = 0,
    Player = 1
}
