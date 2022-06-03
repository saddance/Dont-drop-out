using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DialogRequirements
{
    [Serializable]
    public class ItemReq
    {
        public string tag;
        public int count;

    }

    public int friendshipReq = -1000;
    public char stateReq = '-';
    public BoolValue checkIfCanHelp = BoolValue.None;
    public BoolValue checkIfHelping = BoolValue.None;
    public BoolValue onlyForDefeated = BoolValue.None;
    public ItemReq[] itemReq = new ItemReq[0];

    public bool IsSatisfied(Personality personality)
    {
        if (checkIfCanHelp != BoolValue.None) {
            bool canHelp = personality.asFriend != null && personality.asFriend.CanParticipate();
            return (checkIfCanHelp == BoolValue.True) == canHelp;
        }
        if (checkIfHelping != BoolValue.None)
        {
            bool isHelping = personality.asFriend != null && personality.asFriend.IsParticipating();
            return (checkIfHelping == BoolValue.True) == isHelping;
        }
        if (onlyForDefeated != BoolValue.None)
        {
            bool isDefeated = personality.asEnemy != null && personality.asEnemy.wasDefeated;
            return (onlyForDefeated == BoolValue.True) == isDefeated;
        }
        if (itemReq != null)
        {
            foreach (var item in itemReq)
                if (InventoryMaster.CountWithTag(item.tag) < item.count)
                    return false;
        }

        if (personality.asFriend != null)
        {
            if (personality.asFriend.friendScore < friendshipReq)
                return false;
            if (stateReq != '-' && (personality.asFriend.State == null || personality.asFriend.State.chr != stateReq))
                return false;
        } 
        return true;
    }

    public enum BoolValue
    {
        None,
        True,
        False
    }
}

[Serializable]
public class DialogEffects
{
    public int friendshipAffect = 0;
    public BoolValue participatingAsFriend = BoolValue.None;
    public string[] giveItems;
    public UnitDataGenerator increase;

    public void Effect(Personality personality)
    {
        if (giveItems != null)
            foreach (var item in giveItems)
                InventoryMaster.Add(item);
        if (increase != null)
        {
            GameManager.currentSave.hero.strength += increase.Gen().strength;
            GameManager.currentSave.hero.maxHealth += increase.Gen().maxHealth;
        }
        if (personality.asFriend != null) {
            personality.asFriend.friendScore += friendshipAffect;

            if (participatingAsFriend != BoolValue.None)
                if (!personality.asFriend.TrySetParticipation(participatingAsFriend == BoolValue.True))
                    Debug.LogError("Can't set participation!");
        }
    }

    public enum BoolValue
    {
        None,
        True,
        False
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

    public DialogPData CreateCopy()
    {
        return new DialogPData()
        {
            personalityName = (string)personalityName.Clone(),
            uniqueDialogStarts = uniqueDialogStarts.Select(x => new DialogStart(x.dialogPrefix, x.importance)).ToArray(),
            dailyDialogStarts = dailyDialogStarts.Select(x => new DialogStart(x.dialogPrefix, x.importance)).ToArray(),
            commonDialogStarts = commonDialogStarts.Select(x => new DialogStart(x.dialogPrefix, x.importance)).ToArray()
        };
    }

    // First - all must shown dialogs
    // If no, random from unique & daily
    // Else common, sorted by importance
}