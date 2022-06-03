using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Personality generator", order = 71)]
public class GenerationDescriptor : ScriptableObject
{
    public int count;

    [Header("as Friend")]
    public FriendPData.FriendshipState[] friendshipStates;
    public UnitDataGenerator onBattleAsFriend;

    [Header("as Dialog")]
    public DialogPData dialogData;

    [Header("as Enemy")]
    public UnitDataGenerator[] enemyUnits;
    public HumanAnimType[] enemySupportAnims;
    public DialogEffects effectIfWin;
    
    [Header("as On Map")]
    public HumanAnimType onMap;
    public int[] labels;
    public Sprite noHumanSprite;

    public HumanAnimPData GetOnMap(HumanAnimType onMap)
    {
        switch (onMap)
        {
            case HumanAnimType.kn:
                return HumanAnimPData.KN;
            case HumanAnimType.random:
                return HumanAnimPData.Rand;
            case HumanAnimType.teacher:
                return HumanAnimPData.Teacher;
            default:
                return null;
        }
    }

    public enum HumanAnimType
    {
        no,
        random,
        kn,
        teacher
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
