using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class DialogStart : IComparable<DialogStart>
{
    public int priority = 0;
    public bool happened = false;
    public int lastDayHappened = -1;
    
    public string dialogPrefix;
    public PossibleTimes startType; 

    public int CompareTo(DialogStart other)
    {
        return priority.CompareTo(other.priority);
    }

    public DialogStart(string prefix, PossibleTimes times)
    {
        dialogPrefix = prefix;
        startType = times;
    }

    public bool CanBeUsed()
    {
        if (startType == PossibleTimes.Unlimited)
            return true;
        else if (startType == PossibleTimes.OnceADay)
            throw new NotImplementedException();
        else
            return !happened;
    }

    public void Use()
    {
        happened = true;
        // lastDay !!!
    }

    public enum PossibleTimes
    {
        OnceAGame,
        OnceADay,
        Unlimited
    }
}

[Serializable]
public class DialogPData
{
    public string personalityName;
    public DialogStart[] availableDialogStarts;
}