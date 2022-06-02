using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class FriendPData
{
    [Serializable]
    public class FriendshipState
    {
        public Vector2IntS scoreSegment;
        public char chr;
        public char participatingChr;
        public bool canParticipate;
    }

    public int friendScore;
    public FriendshipState[] states;
    public UnitData onBattle;
    private bool __isParticipating;

    public bool IsParticipating()
    {
        if (State == null)
            return false;
        if (!State.canParticipate)
            __isParticipating = false;
        return __isParticipating;
    }

    public bool CanParticipate()
    {
        if (State == null)
            return false;
        if (!State.canParticipate)
            __isParticipating = false;
        return State.canParticipate && GameManager.currentSave.personalities
                                        .Where(x => x.asFriend != null && x.asFriend.IsParticipating()).Count()
                                        < GameManager.currentSave.maxFriendsOnBattle;
    }

    public bool TrySetParticipation(bool value)
    {
        if (!value || CanParticipate())
        {
            __isParticipating = value;
            return true;
        }
        return false;
    }

    public FriendshipState State => states.FirstOrDefault(s => s.scoreSegment.x <= friendScore
                                                            && friendScore <= s.scoreSegment.y);
}