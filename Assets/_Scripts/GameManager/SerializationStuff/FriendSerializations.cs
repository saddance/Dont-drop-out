using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class FriendshipState
{
    public Vector2IntS scoreSegment;
    public char chr;
    public bool isParticipating;
}

[Serializable]
public class FriendPData
{
    public int friendScore;
    public FriendshipState[] states;
    public UnitData onBattle;

    public FriendshipState State => states.FirstOrDefault(s => s.scoreSegment.x <= friendScore
                                                            && friendScore <= s.scoreSegment.y);

    public bool IsParticipating()
    {
        return true;
    }
}