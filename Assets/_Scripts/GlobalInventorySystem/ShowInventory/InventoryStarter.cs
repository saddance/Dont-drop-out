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
    private bool canBeExited;
    private System.Action<int> afterSelection;
    public string requestedTag { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public void ShowInventory(bool canBeExited = true)
    {
        if (OnInventory)
            throw new System.Exception("Can't show inventory while on inventory!");
        OnInventory = true;
        this.canBeExited = canBeExited;
        currentPanel = Instantiate(inventoryPanelPrefab, transform);

        if (canBeExited)
            currentPanel.GetComponentInChildren<ExitInventoryButton>().Init(() => ExitInventory());
    }

    public void ShowInventoryForGift(string requiredTag, System.Action<int> forWhom, bool canBeExited = true)
    {
        ShowInventory(canBeExited);

        currentPanel.GetComponentInChildren<SelectInventoryButton>().Init(() => SelectOnInventory());
        requestedTag = requiredTag;
        afterSelection = forWhom;
    }

    private void LateUpdate()
    {
        if (!OnInventory)
            return;
        if (canBeExited && (Input.GetKeyDown(KeyCode.Escape)))
            ExitInventory();
    }

    private void ExitInventory()
    {
        OnInventory = false;
        if (currentPanel != null)
            Destroy(currentPanel.gameObject);
        if (afterSelection != null)
            afterSelection(-1);
        afterSelection = null;
    }

    private void SelectOnInventory()
    {
        if (currentPanel.selectedIndex == -1)
            return;

        afterSelection(currentPanel.selectedIndex);
        afterSelection = null;
        ExitInventory();
    }
}
