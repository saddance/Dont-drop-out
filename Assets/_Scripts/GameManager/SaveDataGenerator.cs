using UnityEngine;
using UnityEngine.SceneManagement;

static class SaveDataGenerator
{
    static int enemiesCount = 3;
    static int friendsCount = 3;


    private static void GenEnemyData(Personality personality)
    {
        personality.asEnemy = new EnemyPData();

        int peopleCnt = Random.Range(1, 4);
        personality.asEnemy.people = new UnitData[peopleCnt];
        for (int i = 0; i < peopleCnt; i++)
            personality.asEnemy.people[i] = new UnitData();

        int strength = Random.Range(8, 16);
        for (int i = 0; i < strength; i++)
            personality.asEnemy.people[Random.Range(0, peopleCnt)].strength++;

        int hp = Random.Range(20, 40);
        for (int i = 0; i < hp; i++)
            personality.asEnemy.people[Random.Range(0, peopleCnt)].maxHealth++;
    }

    private static void GenFriendData(Personality personality)
    {
        personality.asFriend = new FriendPData();
        personality.asFriend.self = new UnitData();

        int strength = Random.Range(3, 7);
        for (int i = 0; i < strength; i++)
            personality.asFriend.self.strength++;

        int hp = Random.Range(10, 20);
        for (int i = 0; i < hp; i++)
            personality.asFriend.self.maxHealth++;
    }

    private static Personality GenDefaultPersonality(bool isEnemy)
    {
        var personality = new Personality();

        if (isEnemy)
        {
            personality.hidden = false;
            GenEnemyData(personality);
        }
        else
        {
            personality.hidden = true;
            GenFriendData(personality);
        }

        return personality;
    }

    public static SaveData GenDefaultSave()
    {
        var save = new SaveData()
        {
            playerPosition = new Vector2Int(3, 3),
            saveName = Random.Range(1000000, 10000000).ToString(),
            personalities = new Personality[enemiesCount + friendsCount]                
        };
        for (int i = 0; i < enemiesCount; i++)
            save.personalities[i] = GenDefaultPersonality(true);
        for (int i = 0; i < friendsCount; i++)
            save.personalities[i + enemiesCount] = GenDefaultPersonality(false);

        return save;
    }
}