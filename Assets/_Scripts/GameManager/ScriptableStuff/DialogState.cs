using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DialogOption
{
    public DialogRequirements requirements;
    public DialogEffects effects;    

    [Header("Contains")]
    public bool isHiddenWhenInactive = true;
    public string russianInactiveText;
    public string russianText;
    public OptionType option;

    [Header("Next Dialog/Give Present Parameters")]
    // TODO -> next dialog as full dialog beginning
    public string nextDialogPrefix;

    public enum OptionType
    {
        nextDialog,
        startBattle,
        quit,
        givePresent
    }

    public bool IsActive(Personality personality) => requirements == null || requirements.IsSatisfied(personality);
    public bool IsShown(Personality personality) => IsActive(personality) || !isHiddenWhenInactive;
}

[CreateAssetMenu(menuName="Dialog State", order=70)]
public class DialogState : ScriptableObject
{
    [Header("Requirements")]
    public DialogRequirements requirements;

    [Header("Contains")]
    [TextArea] public string russianText;

    public DialogOption[] options;

    public bool IsActive(Personality personality) => requirements == null || requirements.IsSatisfied(personality);
}

