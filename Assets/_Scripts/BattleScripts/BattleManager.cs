using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager self;
    public PlayerTurnManager ptm;
    public EnemyTurnManager etm;
    [HideInInspector] public int playerUnitsAmount;
    [HideInInspector] public int enemyUnitsAmount;

    public Turn turn;
    public Unit unitPrefab;

    public List<Unit> units;
    public GamePhase gamePhase;
    private int enemyUnitsAlive;
    private int playerUnitsAlive;

    #region Start

    private void Awake()
    {
        self = this;
        units = new List<Unit>();
        turn = Turn.Start;
        gamePhase = GamePhase.Playing;
        GenerateUnits();
    }

    private void GenerateUnits()
    {
        GenerateFriends();
        GenerateEnemies();
    }

    private void GenerateFriends()
    {
        var friends = GameManager.currentSave.personalities
            .Where(p => p.asFriend != null && p.asFriend.IsParticipating())
            .ToList();

        playerUnitsAlive = playerUnitsAmount = friends.Count + 1;

        InstantiateUnit(new UnitInfo(GameManager.currentSave.hero, false,
                new Vector3(-5, 2f * (0 - friends.Count / 2f)), GameManager.currentSave.heroHumanAnim));


        for (var i = 0; i < friends.Count; i++)
        {
            var info = new UnitInfo(friends[i].asFriend.onBattle, false,
                new Vector3(-5, 2f * (i + 1 - friends.Count / 2f)), friends[i].asHumanOnMap);
            InstantiateUnit(info);
        }
    }

    private void GenerateEnemies()
    {
        var battleWith = GameManager.currentSave.personalities[GameManager.currentSave.battleWith];
        var enemies = battleWith.asEnemy.people.ToList();

        enemyUnitsAlive = enemyUnitsAmount = enemies.Count;
        for (var i = 0; i < enemies.Count; i++)
        {
            var info = new UnitInfo(enemies[i], true,
                new Vector3(5, 2f * (i - (enemies.Count - 1) / 2f)),
                i == 0 ? battleWith.asHumanOnMap : battleWith.asEnemy.supportAnims[i-1]);
            InstantiateUnit(info);
        }
    }

    private void InstantiateUnit(UnitInfo info)
    {
        var obj = Instantiate(unitPrefab);
        obj.Init(info);
        units.Add(obj);
    }

    #endregion

    #region Update

    private void Update()
    {
        if (gamePhase != GamePhase.Playing) return;

        switch (turn)
        {
            case Turn.Start:
                turn = Turn.Player;
                ptm = gameObject.AddComponent<PlayerTurnManager>();
                Debug.Log("Started. Now it's the Player's turn");
                return;
        }

        if (enemyUnitsAlive == 0)
        {
            Debug.Log("Player won");
            gamePhase = GamePhase.Win;
            StartCoroutine(FinishBattle(true));
        }

        if (playerUnitsAlive == 0)
        {
            Debug.Log("Player lost");
            gamePhase = GamePhase.Loss;
            StartCoroutine(FinishBattle(false));
        }
    }

    private IEnumerator FinishBattle(bool isWin)
    {
        turn = Turn.Nobody;
        Destroy(ptm);
        Destroy(etm);

        yield return new WaitForSeconds(1.5f);
        GameManager.EndBattle(isWin);
    }

    public void StopPlayerMove()
    {
        if (turn != Turn.Player)
        {
            Debug.LogError("It's not the Player's move");
            return;
        }

        Destroy(ptm);
        etm = gameObject.AddComponent<EnemyTurnManager>();
        Debug.Log("Player passed. Now it's the Enemy's turn");
        turn = Turn.Enemy;
    }

    public void StopEnemyMove()
    {
        if (turn != Turn.Enemy)
        {
            Debug.LogError("It's not the Enemy's move");
            return;
        }

        Destroy(etm);
        ptm = gameObject.AddComponent<PlayerTurnManager>();
        Debug.Log("Enemy passed. Now it's the Player's turn");
        turn = Turn.Player;
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
                enemyUnitsAlive--;
            else
                playerUnitsAlive--;

            Destroy(units[defendIndex].gameObject);
            Update();
        }
    }

    #endregion
}

#region Enums

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

#endregion