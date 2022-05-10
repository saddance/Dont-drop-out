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
        return happened.CompareTo(other.happened);
    }

    public DialogStart(string prefix, PossibleTimes times)
    {
        dialogPrefix = prefix;
        startType = times;
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
    public DialogStart[] availableDialogStarts;
}