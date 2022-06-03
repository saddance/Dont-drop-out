using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStage
{
    menu,
    map,
    battle,
    other
}

public static class GameManager
{
    public static SaveData currentSave;

    private static readonly string mapSceneName = "MmStage";
    private static readonly string menuSceneName = "MenuStage";
    private static readonly string battleSceneName = "BattleStage";

    static GameManager()
    {
        currentSave = SaveDataGenerator.GenDefaultSave();
    }

    public static GameStage Stage
    {
        get
        {
            var scene = SceneManager.GetActiveScene();
            if (scene.name == menuSceneName)
                return GameStage.menu;
            if (scene.name == mapSceneName)
                return GameStage.map;
            if (scene.name == battleSceneName)
                return GameStage.battle;
            return GameStage.other;
        }
    }

    public static void StartScene()
    {
        if (currentSave == null)
            throw new Exception("No currentSave!!!");

        if (currentSave.specialScene != null && currentSave.specialScene != "")
        {
            SceneManager.LoadScene(currentSave.specialScene);
            currentSave.specialScene = null;
        }
        else
            SceneManager.LoadScene(currentSave.battleWith == -1 ? mapSceneName : battleSceneName);
    }

    public static void StartNewGame()
    {
        if (Stage != GameStage.menu)
            throw new Exception("Can't start a new game not from main menu");

        currentSave = SaveDataGenerator.GenDefaultSave();
        StartScene();
    }

    public static void SaveGame(string rename = null)
    {
        if (rename != null)
            currentSave.saveName = rename;

        SavingManager.SaveToFile(currentSave);
    }

    public static void StartBattle(Personality personality)
    {
        if (Stage != GameStage.map)
            throw new Exception("Can't start battle game not from map");

        var index = currentSave.personalities.ToList().IndexOf(personality);
        if (index == -1)
            throw new Exception("No such personality!");

        SaveGame();
        currentSave.battleWith = index;
        StartScene();
    }

    public static void EndBattle(bool isWin)
    {
        if (Stage != GameStage.battle)
            throw new Exception("Can't end battle game not from battle stage");

        if (isWin)
        {
            currentSave.currentDay++;
            
            var def = currentSave.personalities[currentSave.battleWith];
            def.asEnemy.wasDefeated = true;
            def.asEnemy.effectsAfterWin.Effect(def);
            currentSave.startComment = "Вы выиграли битву!";

            currentSave.battleWith = -1;
            StartScene();
        }
        else
        {
            currentSave.specialScene = "Outro";
            currentSave.battleWith = -1;
            StartScene();
        }
    }

    public static void LoadSavedGame(string name = null)
    {
        if (Stage != GameStage.menu)
            throw new Exception("Can't load game not from main menu");

        if (name == null)
            name = SavingManager.GetSaveNames()[0];

        currentSave = SavingManager.LoadFromFile(name);
        StartScene();
    }

    public static void ExitGame(bool withSave = true)
    {
        if (Stage == GameStage.menu)
        {
            Application.Quit();
        }
        else
        {
            if (withSave)
                SavingManager.SaveToFile(currentSave);
            currentSave = null;
            SceneManager.LoadScene(menuSceneName);
        }
    }
}