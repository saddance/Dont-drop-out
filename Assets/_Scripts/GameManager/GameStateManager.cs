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
    static string mmStageName = "MmStage";

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

    public static void NewGame()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
            throw new System.Exception("Can't start a new game not from main menu");

        currentSave = SaveDataHandler.GenDefaultSave();
        SceneManager.LoadScene(mmStageName);
    }

    public static void SaveGame()
    {
        if (currentSave == null)
            throw new System.Exception("Can't start a new game not from game menu");

        SaveDataHandler.UpdateSaveData(currentSave);

        var stream = new FileStream(GetPath(), FileMode.Create);
        var formatter = new BinaryFormatter();
        formatter.Serialize(stream, currentSave);
        stream.Close();

        Debug.Log($"Game saved in {GetPath()}");
    }

    public static void ExitGame(bool withSave = true)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            Application.Quit();
        else
        {
            if (withSave)
                SaveGame();
            currentSave = null;
            SceneManager.LoadScene(0);
        }
    }

    public static void LoadGame(string name = null)
    {
        if (name == null)
            name = GetSaveNames()[0];

        var stream = new FileStream(GetPath(name), FileMode.Open);
        var formatter = new BinaryFormatter();
        currentSave = formatter.Deserialize(stream) as SaveData;
        stream.Close();

        Debug.Log($"Game loaded from {GetPath()}");
        SceneManager.LoadScene(mmStageName);
    }

    public static string[] GetSaveNames()
    {
        var files = Directory.EnumerateFiles(SavePath).OrderByDescending(file => File.GetLastAccessTime(file));
        List<string> names = new List<string>();

        foreach (var file in files)
        {
            var formatter = new BinaryFormatter();
            var stream = new FileStream(file, FileMode.Open);
            try
            {
                if (formatter.Deserialize(stream) is SaveData)
                    names.Add(string.Join("", file.Split('/').Last().Where(x => int.TryParse(new string(x, 1), out int _)).ToArray()));
            }
            catch (System.Exception) { }
            stream.Close();
        }
        
        return names.ToArray();
    }

}
