using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Unit : MonoBehaviour
{
    public UnitInfo Info;
    public SpriteRenderer Renderer;
    private BattleManager bm;
    private PlayerTurnManager ptm;

    void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Renderer.color = Color.black;
    }

    void Start()
    {
        bm = BattleManager.self;
        ptm = PlayerTurnManager.self;
    }

    void Update()
    {
        if (Info == null)
        {
            Debug.Log("Unit is not initialized");
            return;
        }
       

        if (ptm.chosenUnit != -1 && bm.units[ptm.chosenUnit] == this)
            Renderer.color = Color.green;
        else if (ptm.chosenEnemy != -1 && bm.units[ptm.chosenEnemy] == this)
            Renderer.color = Color.red;
        else if(isChosenByMouse())
            Renderer.color = Color.blue;
        else
            Renderer.color = Color.black;
    }

    private bool isChosenByMouse()
    {
        return ptm.ChosenByMouseIndex != -1 && bm.units[ptm.ChosenByMouseIndex] == this 
                                            && ptm.isReady == Info.IsEnemysUnit && bm.Turn == Turn.Player;
    }

    public void Init(UnitInfo info)
    {
        Info = info;
        transform.position = info.Position;
    }
}

public class UnitInfo
{
    public bool IsEnemysUnit;
    public Vector3 Position;
    public int Health;
	public int Strength;
    public bool IsDestroyed { get { return Health <= 0; } }

    public UnitInfo(bool isEnemysUnit, float x, float y, float z)
    {
        IsEnemysUnit = isEnemysUnit;
        Position = new Vector3(x, y, z);
        Health = 100 + Random.Range(-10, 10);
        Strength = 40 + Random.Range(-10, 10);
    }
    
    public UnitInfo(bool isEnemysUnit, Vector3 position)
    {
        IsEnemysUnit = isEnemysUnit;
        Position = position;
        Health = 100 + Random.Range(-10, 10);
        Strength = 40 + Random.Range(-10, 10);
    }
}
