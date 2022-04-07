using UnityEngine;
using System;
using System.Collections.Generic;

public class BattleManager: MonoBehaviour
{
    public static BattleManager self;
    public Unit prefab;

    private List<Tuple<Unit, UnitInfo>> units;

    void Awake()
    {
        self = this;
        units = new List<Tuple<Unit, UnitInfo>>();
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
