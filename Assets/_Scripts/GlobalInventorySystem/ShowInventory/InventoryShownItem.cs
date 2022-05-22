using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryShownItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image image;
    [SerializeField] Text count;
    [SerializeField] Image selectionBorder;

    InventoryPanel parent;

    private int index = -1;

    public void Init(int inventoryIndex, InventoryPanel panel)
    {
        index = inventoryIndex;
        parent = panel;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        parent.ReactOnSelection(index);
    }

    private void LateUpdate()
    {
        var obj = InventoryMaster.At(index);
        if (obj == null)
        {
            image.sprite = null;
            image.color = new Color(0, 0, 0, 0);
            count.text = "";
        }
        else
        {
            var item = InventoryMaster.ItemAt(index);
            image.sprite = item.image;
            image.color = Color.white;
            if (item.MaxAmount > 1)
                count.text = $"{obj.amount}";
            else
                count.text = "";
        }

        selectionBorder.enabled = parent.selectedIndex == index;
    }

}
