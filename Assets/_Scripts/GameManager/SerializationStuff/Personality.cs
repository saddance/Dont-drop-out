using System;
using UnityEngine;

[Serializable]
public class Personality
{
    public bool hidden { get; set; } // used for debug for creating friends
    public EnemyPData asEnemy;
    public FriendPData asFriend;
    public DialogPData asDialog;
    public HumanAnimPData asHumanOnMap;
}   