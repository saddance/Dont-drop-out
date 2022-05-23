using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class FriendPData
{
    int friendScore;

    public UnitData self;

    public bool IsParticipating()
    {
        return true;
    }
}