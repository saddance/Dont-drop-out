using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class FriendPData
{
    public UnitData self;

    public bool IsParticipating()
    {
        return true;
    }
}