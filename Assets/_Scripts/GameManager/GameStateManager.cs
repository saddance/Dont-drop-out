using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class GameStateManager
{
    public static SaveData currentSave;

    private static string SavePath { get { return $"{Application.persistentDataPath}/saves"; } }

    private static string GetPath(string name = null)
    {
        if (name == null)
        {
            if (currentSave == null || currentSave.saveName == null)
                throw new System.Exception("CurrentSave got no name!");
            name = currentSave.saveName;
        }
        return $"{SavePath}/{name}";
    }

    static GameStateManager() {
        if (!Directory.Exists(SavePath))
            Directory.CreateDirectory(SavePath);
    }

    public static void GenDefaultSave()
    {
        currentSave = new SaveData();
        currentSave.saveName = Random.Range(1000000, 10000000).ToString();
        currentSave.personalities = new Personality[5];
        for (int i = 0; i < 5; i++)
            currentSave.personalities[i] = new Personality
            {
                enemy = new EnemyPData
                {
                    maxFriends = Random.Range(0, 2),
                    selfStrength = Random.Range(80, 120)
                }
            };
    }

    public static void NewGame()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
            return;
        GenDefaultSave();
        SceneManager.LoadScene(1);
    }

    public static void SaveGame()
    {
        var stream = new FileStream(GetPath(), FileMode.Create);
        var formatter = new BinaryFormatter();
        formatter.Serialize(stream, currentSave);
        stream.Close();
    }

    public static void ExitGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            Application.Quit();
        else
        {
            SaveGame();
            SceneManager.LoadScene(0);
        }
    }

    public static void LoadGame(string name)
    {
        var stream = new FileStream(GetPath(name), FileMode.Open);
        var formatter = new BinaryFormatter();
        currentSave = formatter.Deserialize(stream) as SaveData;
        stream.Close();

        SceneManager.LoadScene(1);
    }

    public static string[] GetSaveNames(string saves)
    {
        var files = Directory.EnumerateFiles(SavePath);
        List<string> names = new List<string>();
        foreach (var file in files)
        {
            var formatter = new BinaryFormatter();

            var stream = new FileStream(file, FileMode.Open);
            if (formatter.Deserialize(stream) is SaveData)
                names.Add(string.Join("", file.Split('/').Last().Where(x => int.TryParse(new string(x, 1), out int _)).ToArray()));
        }
        return names.ToArray();
    }

}
