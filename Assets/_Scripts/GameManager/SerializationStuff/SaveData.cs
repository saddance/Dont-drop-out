﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SaveData
{
    public string saveName = "1337"; // must be equal to file name
    public Personality[] personalities = new Personality[0]; // can't be null, null in array means it's obstacle
    public HumanAnimPData heroHumanAnim = new HumanAnimPData();

    // On battle or not
    public int battleWith = -1; 
    
    // Map reading options
    public Vector2IntS playerPosition = new Vector2Int(-1, -1); // minus means hasn't assigned
    public Vector2IntS[] mapPositions;
    public InventoryObject[] inventory;
}