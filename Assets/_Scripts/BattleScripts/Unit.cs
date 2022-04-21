using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Unit : MonoBehaviour
{
    public UnitInfo Info;
    public SpriteRenderer Renderer;

    void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Renderer.color = Color.black;   
    }
    void Update()
    {
        if (Info == null)
        {
            Debug.Log("Unit is not initialized");
        }

        if (PlayerTurnManager.self.ChosenUnit != -1 && BattleManager.self.units[PlayerTurnManager.self.ChosenUnit] == this)
            Renderer.color = Color.green;
        else
            Renderer.color = Color.black;
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
