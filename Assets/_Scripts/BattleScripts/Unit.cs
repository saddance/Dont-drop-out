using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Unit : MonoBehaviour
{
    public UnitInfo Info;
    void Update()
    {
        if (Info == null)
        {
            Debug.Log("Unit is not initialized");
        }
    }

    public void Init(UnitInfo info)
    {
        Info = info;
        transform.position = info.Position;
        if (info.IsEnemy)
            GetComponent<SpriteRenderer>().color = Color.black;
    }
}

public class UnitInfo
{
    public bool IsEnemy;
    public Vector3 Position;
    
    public UnitInfo(bool isEnemy, float x, float y, float z)
    {
        IsEnemy = isEnemy;
        Position = new Vector3(x, y, z);
    }
    
    public UnitInfo(bool isEnemy, Vector3 position)
    {
        IsEnemy = isEnemy;
        Position = position;
    }
}
