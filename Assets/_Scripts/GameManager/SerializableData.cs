using System;
using UnityEngine;

[Serializable]
public class UnitData
{
    public int maxHealth;
    public int strength;
}

[Serializable]
public class EnemyPData
{
    public UnitData[] people;
}

[Serializable]
public class FriendPData
{
    public UnitData self;

    public bool IsParticipating()
    {
        return true;
    }
}

[Serializable]
public class Personality
{
    public bool hidden;
    public EnemyPData asEnemy;
    public FriendPData asFriend;
}

[Serializable]
public class SaveData
{
    public string saveName = "1337"; // must be equal to file name
    public int battleWith = -1;
    public Personality[] personalities = new Personality[0]; // can't be null, null in object means it's obstacle

    // Map reading options
    public Vector2IntS playerPosition = new Vector2Int(-1, -1); // minus means hasn't assigned
    public Vector2IntS[] mapPositions;

    public InventoryObject[] inventory;
}

[Serializable]
public class InventoryObject
{
    public string itemName;
    public int Amount;
}

[Serializable]
public class Vector2IntS
{
    public int x;
    public int y;

    public Vector2IntS(Vector2Int v)
    {
        x = v.x;
        y = v.y;
    }

    public Vector2Int GetV()
    {
        return new Vector2Int(x, y);
    }

    public static implicit operator Vector2IntS(Vector2Int v)
    {
        return new Vector2IntS(v);
    }
}

[Serializable]
public class Vector3S
{
    public float x;
    public float y;
    public float z;

    public Vector3S(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public Vector3 Get()
    {
        return new Vector3(x, y, z);
    }

    public static implicit operator Vector3S(Vector3 v)
    {
        return new Vector3S(v);
    }
}