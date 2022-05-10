using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DialogOption
{
    public string russianText;
    
    public OptionType option;

    [Header("Next Dialog Parameters")]
    public string nextDialogPrefix;

    public enum OptionType
    {
        nextDialog,
        startBattle,
        quit
    }
}

[CreateAssetMenu(menuName="Dialog State", order=70)]
public class DialogState : ScriptableObject
{
    public int requiredFriendship;
    public string requiredItemTag;

    [TextArea] public string russianText;

    public DialogOption[] options;
}

