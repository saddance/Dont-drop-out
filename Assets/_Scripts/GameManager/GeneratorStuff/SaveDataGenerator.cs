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
            states = desc.friendshipStates
        };

        personality.asHumanOnMap = desc.GetOnMap();
        personality.asDialog = desc.dialogData;
        personality.asEnemy = new EnemyPData()
        {
            people = desc.enemyUnits.Select(x => x.Gen()).ToArray()
        };
        personality.asMapObject = new MapObjectPData()
        {
            labels = desc.labels
        };

        return personality;
    }

    public static SaveData GenDefaultSave()
    {
        var save = new SaveData
        {
            playerPosition = new Vector2Int(3, 3),
            saveName = null,
            inventory = new InventoryObject[18],
            heroHumanAnim = HumanAnimPData.Rand,
            hero = new UnitData() { maxHealth = 20, strength = 4 }
        };

        for (int i = 0; i < 3; i++)
            save.inventory[i] = new InventoryObject
            {
                itemName = "beer",
                amount = (i == 0 ? 1 : 2)
            };

        List<Personality> personalities = new List<Personality>();
        foreach (var desc in Resources.LoadAll<GenerationDescriptor>("Generation"))
            for (var i = 0; i < desc.count; i++)
                personalities.Add(GenPersonalityByDesc(desc));
        save.personalities = personalities.ToArray();
        return save;
    }
}