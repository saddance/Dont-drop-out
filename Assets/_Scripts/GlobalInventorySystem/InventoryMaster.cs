using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class InventoryMaster
{ 

    static Dictionary<string, Item> items;

    static InventoryMaster()
    {
        items = new Dictionary<string, Item>();
        var its = Resources.LoadAll<Item>("Items");
        foreach (var item in its)
        {
            items[item.name] = item;
        }
        CorrectInventory();
    }

    private static void CorrectInventory()
    {
        var inventory = GameManager.currentSave.inventory;

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
                continue;

            if (!items.ContainsKey(inventory[i].itemName))
            {
                Debug.Log($"No such item {inventory[i].itemName}");
                inventory[i] = null;
                continue;
            }

            var item = items[inventory[i].itemName];
            if (inventory[i].amount == 0)
            {
                Debug.Log($"Zero amount at index [{i}]");
                inventory[i] = null;
            }
            else if (inventory[i].amount > item.MaxAmount)
            {
                Debug.Log($"More than maximum amount at index [{i}]");
                inventory[i].amount = item.MaxAmount;
            }

        }
    }

    public static InventoryObject At(int index)
    {
        if (GameManager.currentSave == null)
            return null;
        if (!(0 <= index && index < GameManager.currentSave.inventory.Length))
            return null;

        CorrectInventory();
        return GameManager.currentSave.inventory[index];
    }

    public static Item ItemAt(int index)
    {
        var name = At(index)?.itemName;
        return name != null && items.ContainsKey(name) ? items[name] : null;
    }

    public static bool TryPutFrom(int fromIndex, int toIndex)
    {
        var item = ItemAt(fromIndex);
        if (item == null)
            return false;

        var inventory = GameManager.currentSave.inventory;
        if (inventory[toIndex] == null)
        {
            inventory[toIndex] = new InventoryObject
            {
                itemName = inventory[fromIndex].itemName,
                amount = 0
            };
        }
        if (inventory[toIndex].itemName != inventory[fromIndex].itemName)
            return false;

        var delta = Mathf.Min(item.MaxAmount - inventory[toIndex].amount, inventory[fromIndex].amount);
        inventory[toIndex].amount += delta;
        inventory[fromIndex].amount -= delta;

        CorrectInventory();
        return true;
    }

    public static void Add(string itemName)
    {
        if (!items.ContainsKey(itemName))
            throw new System.Exception($"item {itemName} does not exists");

        var inventory = GameManager.currentSave.inventory;

        int freeIndex = -1;
        for (int i= inventory.Length - 1; i >=0; i--)
        {
            var item = ItemAt(i);
            if (item == null)
                freeIndex = i;
            else if (item.name == itemName && inventory[i].amount < item.MaxAmount)
            {
                inventory[i].amount++;
                return;
            }
        }
        if (freeIndex == -1)
            Debug.LogError("Inventory is full!");
        else
        {
            inventory[freeIndex] = new InventoryObject() { amount = 1, itemName = itemName };
        }
    }

    public static int CountWithTag(string tag)
    {
        var inventory = GameManager.currentSave.inventory;

        int ans = 0;
        for (int i = inventory.Length - 1; i >= 0; i--)
        {
            var item = ItemAt(i);
            if (item != null && item.tags.Contains(tag))
                    ans += inventory[i].amount;
        }
        return ans;
    }
}
