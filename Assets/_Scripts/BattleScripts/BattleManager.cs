using UnityEngine;
using System;
using System.Collections.Generic;

public class BattleManager: MonoBehaviour
{
    public static BattleManager self;
    public Unit prefab;
    public Mover mover { get; private set; }
    public List<Unit> units;
    private List<UnitInfo> _unitsInfo;
    public List<UnitInfo> unitsInfo
    {
        get { return new List<UnitInfo>(_unitsInfo);}
    }

    void Awake()
    {
        self = this;
        units = new List<Unit>();
        _unitsInfo = new List<UnitInfo>();
        mover = Mover.Start;
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
        for (int i = 0; i < 3; i++)
        {
            var info = new UnitInfo(false, -5, 2 * i, 0);
            var obj = Instantiate(prefab);
            obj.Init(info);

            units.Add(obj);
            _unitsInfo.Add(info);
        }
        for (int i = 0; i < 3; i++)
        {
            var info = new UnitInfo(true, 5, 2 * i, 0);
            var obj = Instantiate(prefab);
            obj.Init(info);

            units.Add(obj);
            _unitsInfo.Add(info);
        }
    }
}

public enum Mover
{
    Enemy,
    Start,
    Player
}
