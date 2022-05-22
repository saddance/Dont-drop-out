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


        if (ptm.selectedUnit != -1 && bm.units[ptm.selectedUnit] == this)
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
        else if (ptm.selectedEnemy != -1 && bm.units[ptm.selectedEnemy] == this)
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
        return ptm.SelectedIndex != -1 && bm.units[ptm.SelectedIndex] == this
                                            && (Info.IsEnemysUnit && ptm.selected == Selected.Unit || !Info.IsEnemysUnit && ptm.selected == Selected.Nodody)
                                            && bm.turn == Turn.Player;
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