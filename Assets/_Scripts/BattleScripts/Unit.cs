using UnityEngine;

public class Unit : MonoBehaviour
{
    public SpriteRenderer Renderer;
    public Sprite UnitSprite;
    public Sprite EnemySprite;
    public Sprite ChosenUnitByMouseSprite;
    public Sprite ChosenEnemyByMouseSprite;
    public Sprite UnitAttackingSprite;
    public Sprite EnemyAttackingSprite;
    public Sprite UnitAttackedSprite;
    public Sprite EnemyAttackedSprite;
    private BattleManager bm;
    public UnitInfo Info;
    private PlayerTurnManager ptm;

    private void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
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
        {
            if (bm.turn == Turn.Player)
            {
                Renderer.sprite = UnitAttackingSprite;
            }
            else
            {
                Renderer.sprite = UnitAttackedSprite;
            }
        }
        else if (ptm.chosenEnemy != -1 && bm.units[ptm.chosenEnemy] == this)
        {
            if (bm.turn == Turn.Player)
            {
                Renderer.sprite = EnemyAttackedSprite;
            }
            else
            {
                Renderer.sprite = EnemyAttackingSprite;
            }
        }
        else if (isChosenByMouse())
        {
            if (!Info.IsEnemysUnit)
            {
                Renderer.sprite = ChosenUnitByMouseSprite;
            }
            else
            {
                Renderer.sprite = ChosenEnemyByMouseSprite;
            }
        }
        else
        {
            if (!Info.IsEnemysUnit)
            {
                Renderer.sprite = UnitSprite;
            }
            else
            {
                Renderer.sprite = EnemySprite;
            }
        }
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