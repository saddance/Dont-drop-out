using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] GridLayoutGroup layout;
    [SerializeField] InventoryShownItem itemPrefab;
    void Start()
    {
        for (int i = 0; i < GameManager.currentSave.inventory.Length; i++)
        {
            var item = Instantiate(itemPrefab, layout.transform);
            item.Init(i);
        }
    }

}
