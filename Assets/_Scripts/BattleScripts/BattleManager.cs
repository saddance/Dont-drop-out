using UnityEngine;
using System;
using System.Collections.Generic;

public class BattleManager: MonoBehaviour
{
    public static BattleManager self;
    public Unit prefab;
    public Mover mover { get; private set; }
    private List<Tuple<Unit, UnitInfo>> units;

    void Awake()
    {
        self = this;
        units = new List<Tuple<Unit, UnitInfo>>();
        mover = Mover.start;
    }

    void Update()
    {
        if (mover == Mover.start)
        {
            mover = Mover.player;
            Debug.Log("Start -> Player move");
            return;
        }
        if (mover == Mover.enemy)
        {
            mover = Mover.player;
            Debug.Log("Enemy -> Player move");
            return;
        }
    }

    public void StopPlayerMove()
    {
        if (mover != Mover.player)
        {
            Debug.LogError("Not player move");
            return;
        }
        mover = Mover.enemy;
    }

    public void StopEnemyMove()
    {
        if (mover != Mover.enemy)
        {
            Debug.LogError("Not enemy move");
            return;
        }

        mover = Mover.player;
    }

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            var info = new UnitInfo(false, -5, 2 * i, 0);
            var obj = Instantiate(prefab);
            obj.Init(info);

            units.Add(Tuple.Create(obj, info));
        }
        for (int i = 0; i < 3; i++)
        {
            var info = new UnitInfo(true, 5, 2 * i, 0);
            var obj = Instantiate(prefab);
            obj.Init(info);

            units.Add(Tuple.Create(obj, info));
        }
    }
}

public enum Mover
{
    enemy = -1,
    start = 0,
    player = 1
}
