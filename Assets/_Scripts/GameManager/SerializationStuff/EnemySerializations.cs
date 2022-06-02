using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class EnemyPData
{
    [Tooltip("The first will be attached to asMapObject")]
    public UnitData[] people;
    public HumanAnimPData[] supportAnims;
    public bool wasDefeated = false;
}
