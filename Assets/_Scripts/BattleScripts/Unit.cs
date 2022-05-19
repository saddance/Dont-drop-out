using UnityEngine;

public class Unit : MonoBehaviour
{
    public SpriteRenderer Renderer;
    private BattleManager bm;
    public UnitInfo Info;

    #region Start

    private void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Renderer.color = Color.black;
        bm = BattleManager.self;
    }

    public void Init(UnitInfo info)
    {
        Info = info;
        transform.position = info.Position;
    }

    #endregion

    #region Update

    private void Update()
    {
        if (Info == null)
        {
            Debug.Log("Unit is not initialized");
            return;
        }


        if (bm.ptm.chosenUnit != -1 && bm.units[bm.ptm.chosenUnit] == this)
            Renderer.color = Color.green;
        else if (bm.ptm.chosenEnemy != -1 && bm.units[bm.ptm.chosenEnemy] == this)
            Renderer.color = Color.red;
        else if (isChosenByMouse())
            Renderer.color = Color.blue;
        else
            Renderer.color = Color.black;
    }

    private bool isChosenByMouse()
    {
        return bm.ptm.ChosenByMouseIndex != -1 && bm.units[bm.ptm.ChosenByMouseIndex] == this
                                               && bm.ptm.isReady == Info.IsEnemysUnit && bm.turn == Turn.Player;
    }

    #endregion
}

public class UnitInfo
{
    public int Health;
    public bool IsEnemysUnit;
    public int MaxHealth;
    public Vector3 Position;
    public int Strength;


    public UnitInfo(UnitData data, bool isEnemy, Vector3 position)
    {
        IsEnemysUnit = isEnemy;
        Position = position;
        MaxHealth = Health = data.maxHealth;
        Strength = data.strength;
    }

    public bool IsDestroyed => Health <= 0;
}