using UnityEngine;

public static class SaveDataHandler
{
    public static SaveData GenDefaultSave()
    {
        var save = new SaveData()
        {
            playerPosition = new Vector2Int(3, 3),
            saveName = Random.Range(1000000, 10000000).ToString(),
            personalities = new Personality[5],
        };
        for (int i = 0; i < 5; i++)
            save.personalities[i] = new Personality
            {
                enemy = new EnemyPData
                {
                    maxFriends = Random.Range(0, 2),
                    selfStrength = Random.Range(80, 120)
                }
            };
        return save;
    }

    public static void UpdateSaveData(SaveData save)
    {
        var player = GameObject.FindGameObjectWithTag("Player").transform.position;
        save.playerPosition = new Vector2Int(Mathf.RoundToInt(player.x), Mathf.RoundToInt(player.y));
        
    }
}