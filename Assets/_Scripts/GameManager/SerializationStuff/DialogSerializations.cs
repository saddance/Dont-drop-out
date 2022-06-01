using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DialogRequirements
{
    public int friendshipReq = -1000;
    public char stateReq = '-';

    public bool IsSatisfied(Personality personality)
    {
        if (personality.asFriend != null)
        {
            if (personality.asFriend.friendScore < friendshipReq)
                return false;
            if (stateReq != '-' && (personality.asFriend.State == null || personality.asFriend.State.chr != stateReq))
                return false;
        } 
        return true;
    }
}

[Serializable]
public class DialogEffects
{
    public int friendshipAffect = 0;

    public void Effect(Personality personality)
    {
        if (personality.asFriend != null)
        {
            personality.asFriend.friendScore += friendshipAffect;
        }
    }
}

[Serializable]
public class DialogStart
{
    public string dialogPrefix;
    public Importance importance; // makes sense only on unique or daily dialogs
    public int lastDayUsed = -1;

    public DialogStart(string prefix, Importance importance = Importance.Common)
    {
        dialogPrefix = prefix;
        this.importance = importance;
    }

    public enum Importance
    {
        MustBeShown,
        Common
    }
}

[Serializable]
public class DialogPData
{
    public string personalityName = "MISSING_NO";
    public int lastDayUsed = -1;

    public DialogStart[] uniqueDialogStarts = new DialogStart[0];
    public DialogStart[] dailyDialogStarts = new DialogStart[0];
    public DialogStart[] commonDialogStarts = new DialogStart[0];

    // First - all must shown dialogs
    // If no, random from unique & daily
    // Else common, sorted by importance
}