using UnityEngine;
using System.Linq;
using System.Collections.Generic;

internal static class SaveDataGenerator
{
    private static Personality GenPersonalityByDesc(GenerationDescriptor desc)
    {
        var personality = new Personality();

        personality.asFriend = new FriendPData()
        {
            onBattle = desc.onBattleAsFriend.Gen(),
            friendScore = 0,
            states = desc.friendshipStates,
        };

        personality.asHumanOnMap = desc.GetOnMap(desc.onMap);
        personality.asDialog = desc.dialogData.CreateCopy();
        personality.asEnemy = new EnemyPData()
        {
            people = desc.enemyUnits.Select(x => x.Gen()).ToArray()
        };
        if (desc.enemySupportAnims != null)
            personality.asEnemy.supportAnims = desc.enemySupportAnims.Select(x => desc.GetOnMap(x)).ToArray();
        

        personality.asMapObject = new MapObjectPData()
        {
            labels = desc.labels,
            noHumanSprite = null
        };
        if (desc.noHumanSprite != null)
            personality.asMapObject.noHumanSprite = desc.noHumanSprite.name;

        return personality;
    }

    public static SaveData GenDefaultSave()
    {
        var save = new SaveData
        {
            saveName = null,
            inventory = new InventoryObject[18],
            heroHumanAnim = HumanAnimPData.Rand,
            hero = new UnitData() { maxHealth = 40, strength = 5 }
        };

        save.inventory[0] = new InventoryObject
        {
            itemName = "beer",
            amount = 2
        };

        List<Personality> personalities = new List<Personality>();
        foreach (var desc in Resources.LoadAll<GenerationDescriptor>("Generation"))
            for (var i = 0; i < desc.count; i++)
                personalities.Add(GenPersonalityByDesc(desc));
        save.personalities = personalities.ToArray();
        return save;
    }
}