using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Personality generator", order = 71)]
public class GenerationDescriptor : ScriptableObject
{
    public int count;

    [Header("as Friend")]
    public FriendshipState[] friendshipStates;
    public UnitDataGenerator onBattleAsFriend;

    [Header("as Dialog")]
    public DialogPData dialogData;

    [Header("as Enemy")]
    public UnitDataGenerator[] enemyUnits;
    
    [Header("as On Map")]
    public HumanAnimType onMap;

    public HumanAnimPData GetOnMap()
    {
        switch (onMap)
        {
            case HumanAnimType.enemy:
                return HumanAnimPData.Enemy;
            case HumanAnimType.random:
            default:
                return HumanAnimPData.Rand;
        }
    }

    public enum HumanAnimType
    {
        random,
        enemy
    }
}

[System.Serializable]
public class UnitDataGenerator
{
    public Vector2IntS strengthPoss;
    public Vector2IntS healthPoss;

    public UnitData Gen()
    {
        return new UnitData() {
            maxHealth=Random.Range(healthPoss.x, healthPoss.y+1),
            strength=Random.Range(strengthPoss.x, strengthPoss.y+1),
        };
    }
}
