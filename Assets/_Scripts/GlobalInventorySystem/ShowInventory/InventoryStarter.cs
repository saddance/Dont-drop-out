using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryStarter : MonoBehaviour
{
    public static InventoryStarter instance;
    public bool OnInventory { get; private set; } = false;

    [SerializeField] private InventoryPanel inventoryPanelPrefab;
    private InventoryPanel currentPanel;


    private void Awake()
    {
        instance = this;
    }

    public void ShowInventory()
    {
        OnInventory = true;
        currentPanel = Instantiate(inventoryPanelPrefab, transform);

        currentPanel.GetComponentInChildren<ExitInventoryButton>().Init(() => ExitInventory());
    }

    private void LateUpdate()
    {
        if (!OnInventory)
            return;
        if (Input.GetKeyDown(KeyCode.Escape))
            ExitInventory();
    }

    private void ExitInventory()
    {
        OnInventory = false;
        if (currentPanel != null)
            Destroy(currentPanel.gameObject);
    }
}