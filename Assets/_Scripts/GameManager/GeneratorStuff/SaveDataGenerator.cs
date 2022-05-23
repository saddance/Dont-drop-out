using UnityEngine;

internal static class SaveDataGenerator
{
    private static readonly int enemiesCount = 3;
    private static readonly int friendsCount = 0;

    private static Personality GenEnemyData()
    {
        Personality personality = new Personality();
        personality.asEnemy = new EnemyPData();

        var peopleCnt = Random.Range(1, 4);
        personality.asEnemy.people = new UnitData[peopleCnt];
        for (var i = 0; i < peopleCnt; i++)
            personality.asEnemy.people[i] = new UnitData();

        var strength = Random.Range(8, 16);
        for (var i = 0; i < strength; i++)
            personality.asEnemy.people[Random.Range(0, peopleCnt)].strength++;

        var hp = Random.Range(20, 40);
        for (var i = 0; i < hp; i++)
            personality.asEnemy.people[Random.Range(0, peopleCnt)].maxHealth++;

        personality.asDialog = new DialogPData()
        {
            availableDialogStarts = new DialogStart[1],
            personalityName = "КН-щик"
        };
        personality.asDialog.availableDialogStarts[0] = new DialogStart("enemy-greet", DialogStart.PossibleTimes.Unlimited);

        personality.asHumanOnMap = HumanAnimPData.Enemy;

        return personality;
    }

    //private static Personality GenFriendData()
    //{
    //    var personality = new Personality();

    //    personality.asFriend = new FriendPData();
    //    personality.asFriend.self = new UnitData();

    //    var strength = Random.Range(3, 7);
    //    for (var i = 0; i < strength; i++)
    //        personality.asFriend.self.strength++;

    //    var hp = Random.Range(10, 20);
    //    for (var i = 0; i < hp; i++)
    //        personality.asFriend.self.maxHealth++;

    //    return personality;
    //}

    public static SaveData GenDefaultSave()
    {
        var save = new SaveData
        {
            playerPosition = new Vector2Int(3, 3),
            saveName = Random.Range(1000000, 10000000).ToString(),
            personalities = new Personality[enemiesCount + friendsCount],
            inventory = new InventoryObject[18],
            heroHumanAnim = HumanAnimPData.Rand
        };

        for (int i = 0; i < 3; i++)
            save.inventory[i] = new InventoryObject
            {
                itemName = "beer",
                amount = (i == 0 ? 1 : 2)
            };

        for (var i = 0; i < enemiesCount; i++)
            save.personalities[i] = GenEnemyData();
        //for (var i = 0; i < friendsCount; i++)
        //    save.personalities[i + enemiesCount] = GenDefaultPersonality(false);

        return save;
    }
}