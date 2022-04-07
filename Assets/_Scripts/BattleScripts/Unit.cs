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
        this.Info = info;
    }
}

public class UnitInfo
{
    public bool IsEnemy;
    public Vector3 Position;
    
    public UnitInfo(bool isEnemy, float x, float y, float z)
    {
        this.IsEnemy = isEnemy;
        this.Position = new Vector3(x, y, z);
    }
    
    public UnitInfo(bool isEnemy, Vector3 position)
    {
        this.IsEnemy = isEnemy;
        this.Position = position;
    }
}
