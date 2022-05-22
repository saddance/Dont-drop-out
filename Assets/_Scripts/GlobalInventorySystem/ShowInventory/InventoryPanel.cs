using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GridLayoutGroup layout;
    [SerializeField] InventoryShownItem itemPrefab;

    public int selectedIndex { get; private set; } = -1;

    void Start()
    {
        for (int i = 0; i < GameManager.currentSave.inventory.Length; i++)
        {
            var item = Instantiate(itemPrefab, layout.transform);
            item.Init(i, this);
        }
    }

    public void ReactOnSelection(int index)
    {
        if (selectedIndex == -1)
        {
            if (InventoryMaster.At(index) != null)
                selectedIndex = index;
        }
        else if (selectedIndex == index)
            selectedIndex = -1;
        else if (InventoryMaster.TryPutFrom(selectedIndex, index))
            selectedIndex = -1;
        else
            selectedIndex = index;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selectedIndex = -1;
    }
}
