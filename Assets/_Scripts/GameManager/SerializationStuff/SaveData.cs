using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int currentDay = 1;
    public int maxFriendsOnBattle = 4;
    public string specialScene = "Intro";

    public string saveName = null; // must be equal to file name
    public Personality[] personalities = new Personality[0]; // can't be null, null in array means it's obstacle
    public HumanAnimPData heroHumanAnim = HumanAnimPData.Rand;
    public UnitData hero; // TMP

    // On battle or not
    public int battleWith = -1; 
    
    // Map reading options
    public Vector2IntS playerPosition = null; // minus means hasn't assigned
    public Vector2IntS[] mapPositions;
    public InventoryObject[] inventory;
}