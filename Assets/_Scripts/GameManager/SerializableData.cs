using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class EnemyPData
{
    //public double playerStrengthKoef;
    public int selfStrength;
    public int maxFriends;
}


[Serializable]
public class Personality
{
    public int friendship = 0;
    public EnemyPData enemy;
}

[Serializable]
public class SaveData
{
    public string saveName;
    public Vector2IntS playerPosition = new Vector2Int(0, 0);
    public Personality[] personalities = null;
    private Vector2IntS[] positions = null;

    public Vector2IntS[] Positions
    {
        get
        {
            if (positions == null)
            {
                throw new Exception("Positions is null!!");
            }
            return positions;
        }
    }
}

[Serializable]
public class Vector2IntS
{
    public int x;
    public int y;

    public Vector2IntS(Vector2Int v)
    {
        x = v.x; y = v.y;
    }

    public Vector2Int Get()
    {
        return new Vector2Int(x, y);
    }

    public static implicit operator Vector2IntS(Vector2Int v)
    {
        return new Vector2IntS(v);
    }
}