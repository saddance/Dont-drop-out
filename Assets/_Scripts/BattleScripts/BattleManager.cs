using UnityEngine;

public class BattleManager: MonoBehaviour
{
    public static BattleManager self;
    public Unit prefab;
    void Awake()
    {
        self = this;
    }

    void Start()
    {
        Instantiate(prefab);
    }
}
