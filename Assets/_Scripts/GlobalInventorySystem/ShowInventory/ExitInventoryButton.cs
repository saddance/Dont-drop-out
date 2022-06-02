using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ExitInventoryButton : MonoBehaviour
{
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Init(System.Action action)
    {
        button.onClick.AddListener(delegate { action(); });
        foreach (var component in GetComponentsInChildren<MonoBehaviour>())
            component.enabled = true;
    }
}
