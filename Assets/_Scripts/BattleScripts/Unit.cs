using UnityEngine;

public class Unit : MonoBehaviour
{
    public SpriteRenderer Renderer;
    private BattleManager bm;
    public UnitInfo Info;
    private PlayerTurnManager ptm;

    private void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Renderer.color = Color.black;
    }

    private void Start()
    {
        bm = BattleManager.self;
        ptm = PlayerTurnManager.self;
    }

    private void Update()
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
        else if (isChosenByMouse())
            Renderer.color = Color.blue;
        else
            Renderer.color = Color.black;
    }

    private bool isChosenByMouse()
    {
        return ptm.ChosenByMouseIndex != -1 && bm.units[ptm.ChosenByMouseIndex] == this
                                            && ptm.isReady == Info.IsEnemysUnit && bm.turn == Turn.Player;
    }

    public void Init(UnitInfo info)
    {
        Info = info;
        transform.position = info.Position;
    }
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