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
    public Vector2Int playerPosition = new Vector2Int(0, 0);
    public Personality[] personalities = null;
    private Vector2Int[] positions = null;

    public Vector2Int[] Positions
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