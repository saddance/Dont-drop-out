using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SavingManager
{
    static SavingManager()
    {
        if (!Directory.Exists(SavePath))
            Directory.CreateDirectory(SavePath);
    }

    private static string SavePath => $"{Application.persistentDataPath}/saves";

    private static string GetPath(string name)
    {
        return $"{SavePath}/{name}";
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

        var saveData = formatter.Deserialize(stream) as SaveData;
        stream.Close();

        return saveData;
    }

    public static string[] GetSaveNames()
    {
        var files = Directory.EnumerateFiles(SavePath).OrderByDescending(file => File.GetLastAccessTime(file));
        var names = new List<string>();

        foreach (var file in files)
        {
            var formatter = new BinaryFormatter();
            var stream = new FileStream(file, FileMode.Open);
            try
            {
                if (formatter.Deserialize(stream) is SaveData)
                    names.Add(string.Join("", file.Split('/', '\\').Last().ToArray()));
            }
            catch (Exception)
            {
            }

            stream.Close();
        }

        return names.ToArray();
    }
}