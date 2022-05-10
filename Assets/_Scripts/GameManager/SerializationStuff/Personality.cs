﻿using System;
using UnityEngine;

[Serializable]
public class Personality
{
    public bool hidden; // used for debug for creating friends
    public EnemyPData asEnemy;
    public FriendPData asFriend;
    public DialogPData asDialog;
}