using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SavingManager
{
    private static string SavePath { get { return $"{Application.persistentDataPath}/saves"; } }

    private static string GetPath(string name)
    {
        return $"{SavePath}/{name}";
    }

    static SavingManager() {
        if (!Directory.Exists(SavePath))
            Directory.CreateDirectory(SavePath);
    }

    public static void SaveToFile(SaveData saveData)
    {
        var stream = new FileStream(GetPath(saveData.saveName), FileMode.Create);
        var formatter = new BinaryFormatter();
        formatter.Serialize(stream, saveData);
        stream.Close();

        Debug.Log($"Game saved in {GetPath(saveData.saveName)}");
    }

    public static SaveData LoadFromFile(string name)
    {
        var stream = new FileStream(GetPath(name), FileMode.Open);
        var formatter = new BinaryFormatter();

        SaveData saveData = formatter.Deserialize(stream) as SaveData;
        stream.Close();
        
        return saveData;
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
                    names.Add(string.Join("", file.Split('/', '\\').Last().ToArray()));
            }
            catch (System.Exception) { }
            stream.Close();
        }
        
        return names.ToArray();
    }

}
