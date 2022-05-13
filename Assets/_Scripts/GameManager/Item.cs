using System;
using UnityEngine;


[CreateAssetMenu(fileName = "New item", menuName = "Item")]
public class Item : ScriptableObject
{ 
    public Sprite image;
    public int MaxAmount;
}