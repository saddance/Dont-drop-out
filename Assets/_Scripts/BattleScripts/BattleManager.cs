using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class BattleManager: MonoBehaviour
{
    [Header("Units amount constants")]
    [SerializeField] public int playerUnitsAmount = 5;
    [SerializeField] public int enemyUnitsAmount = 5;
    
    private int playerUnitsAlive;
    private int enemyUnitsAlive;
    
    public static BattleManager self;
    public Unit prefab;
    
    public Turn Turn { get; private set; }
    public List<Unit> units;
    public GamePhase gamePhase;

    void Awake()
    {
        self = this;
        units = new List<Unit>();
        Turn = Turn.Start;
        gamePhase = GamePhase.Playing;
        playerUnitsAlive = playerUnitsAmount;
        enemyUnitsAlive = enemyUnitsAmount;
        GenerateUnits();
    }

    void Update()
    {
        if (gamePhase != GamePhase.Playing)
        {
            return;
        }
        
        switch (Turn)
        {
            case Turn.Start:
                Turn = Turn.Player;
                Debug.Log("Started. Now it's the Player's turn");
                return;
        }

        if (enemyUnitsAlive == 0)
        {
            Debug.Log("Player won");
            Turn = Turn.Nobody;
            gamePhase = GamePhase.Win;
        }

        if (playerUnitsAlive == 0)
        {
            Debug.Log("Player lost");
            Turn = Turn.Nobody;
            gamePhase = GamePhase.Loss;
        }
    }

    public void StopPlayerMove()
    {
        if (Turn != Turn.Player)
        {
            Debug.LogError("It's not the Player's move");
            return;
        }

        Debug.Log("Player passed. Now it's the Enemy's turn");
        Turn = Turn.Enemy;
    }

    public void StopEnemyMove()
    {
        if (Turn != Turn.Enemy)
        {
            Debug.LogError("It's not the Enemy's move");
            return;
        }
        Debug.Log("Enemy passed. Now it's the Player's turn");
        Turn = Turn.Player;
    }

    void GenerateUnits()
    {
        for (int i = 0; i < playerUnitsAmount; i++)
        {
            var pos = new Vector3(-5, 1.5f * (i - (playerUnitsAmount - 1) / 2f), 0);
            var info = new UnitInfo(false, pos);
            var obj = Instantiate(prefab);
            obj.Init(info);

            units.Add(obj);
        }
        for (int i = 0; i < enemyUnitsAmount; i++)
        {
            var pos = new Vector3(5, 1.5f * (i - (enemyUnitsAmount - 1) / 2f), 0);
            var info = new UnitInfo(true, pos);
            var obj = Instantiate(prefab);
            obj.Init(info);

            units.Add(obj);
        }
    }

    public void Fight(int attackIndex, int defendIndex)
    {
        var attacker = units[attackIndex];
        var defender = units[defendIndex];
        if (attacker == null || defender == null)
        {
            Debug.Log(units.Count);
            throw new Exception("Who is absent?");
        }
        defender.Info.Health -= attacker.Info.Strength;
        if (defender.Info.IsDestroyed)
        {
            if (defender.Info.IsEnemysUnit)
            {
                enemyUnitsAlive--;
            }
            else
            {
                playerUnitsAlive--;
            }

            Destroy(units[defendIndex].gameObject);
        }
    }
}

public enum Turn
{
    Enemy,
    Start,
    Player,
    Nobody
}

public enum GamePhase
{
    Win,
    Playing,
    Loss
}
