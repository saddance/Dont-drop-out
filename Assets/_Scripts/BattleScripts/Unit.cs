using UnityEngine;

public class Unit : MonoBehaviour
{
    private UnitSelection[] selections;
    private BattleManager bm;
    public UnitInfo Info;

    private void Awake()
    {
        selections = GetComponentsInChildren<UnitSelection>();
        foreach (var sel in selections)
            sel.color = new Color(0, 0, 0, 0);
        bm = BattleManager.self;
    }

    public void Init(UnitInfo info)
    {
        Info = info;
        transform.position = info.Position;
    }

    #region Update

    private void Update()
    {
        if (Info == null)
        {
            Debug.Log("Unit is not initialized");
            return;
        }


        if (bm.ptm.chosenUnit != -1 && bm.units[bm.ptm.chosenUnit] == this)
            foreach (var renderer in selections)
                renderer.color = new Color(0, 0.7f, 0);
        else if (bm.ptm.chosenEnemy != -1 && bm.units[bm.ptm.chosenEnemy] == this)
            foreach (var renderer in selections)
                renderer.color = new Color(0.7f, 0, 0);
        else if (isChosenByMouse())
            foreach (var renderer in selections)
                renderer.color = new Color(0, 0, 0.7f);
        else
            foreach (var renderer in selections)
                renderer.color = new Color(0, 0, 0, 0);
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
    public HumanAnimPData animData;

    public UnitInfo(UnitData data, bool isEnemy, Vector3 position, HumanAnimPData animData)
    {
        IsEnemysUnit = isEnemy;
        Position = position;
        MaxHealth = Health = data.maxHealth;
        Strength = data.strength;
        this.animData = animData;
    }

    public bool IsDestroyed => Health <= 0;
}