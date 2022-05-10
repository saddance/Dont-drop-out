using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class DialogStart : IComparable<DialogStart>
{
    public int happened = 0;
    public string dialogName;

    public int CompareTo(DialogStart other)
    {
        return happened.CompareTo(other.happened);
    }

    public DialogStart(string dialogName)
    {
        this.dialogName = dialogName;
        happened = 0;
    }
}

[Serializable]
public class DialogPData
{
    public DialogStart[] availableDialogNames;
}