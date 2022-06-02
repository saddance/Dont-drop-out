using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Item", order = 70)]
public class Item : ScriptableObject
{ 
    public Sprite image;
    public int MaxAmount;
    public string[] tags;
}