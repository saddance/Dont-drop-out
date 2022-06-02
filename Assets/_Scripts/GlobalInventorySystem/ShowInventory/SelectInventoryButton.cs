using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectInventoryButton : MonoBehaviour
{
    [SerializeField] private Color disabledColor; 
    private Button button;
    private InventoryPanel panel;

    private void Awake()
    {
        button = GetComponent<Button>();
        panel = GetComponentInParent<InventoryPanel>();
    }

    public void Init(System.Action action)
    {
        button.onClick.AddListener(delegate { action(); });
        foreach (var component in GetComponentsInChildren<MonoBehaviour>())
            component.enabled = true;
    }

    private void LateUpdate()
    {
        if (panel.selectedIndex == -1)
        {
            button.interactable = false;
            button.image.color = disabledColor;
        }
        else
        {
            button.interactable = true;
            button.image.color = Color.white;
        }
    }
}
