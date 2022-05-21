using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryShownItem : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Text count;

    private int index = -1;
    private InventoryObject obj
    {
        get
        {
            if (GameManager.currentSave == null)
                return null;
            if (0 <= index && index < GameManager.currentSave.inventory.Length)
                return GameManager.currentSave.inventory[index];
            else
                return null;
        }
    }

    public void Init(int inventoryIndex)
    {
        index = inventoryIndex;
    }

    private void LateUpdate()
    {
        if (obj == null)
        {
            image.sprite = null;
            image.color = new Color(0, 0, 0, 0);
            count.text = "";
        }
        else
        {
            var item = Resources.Load<Item>($"Items/{obj.itemName}");
            image.sprite = item.image;
            image.color = Color.white;
            if (item.MaxAmount > 1)
                count.text = $"{obj.amount}";
            else
                count.text = "";
        }
    }

}
