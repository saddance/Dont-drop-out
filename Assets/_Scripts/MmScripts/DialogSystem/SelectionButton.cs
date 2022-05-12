using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionButton : MonoBehaviour
{
    [SerializeField] float additionalSize;
    Text text;
    Image image;
    DialogOption option;

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
        image = GetComponentInChildren<Image>();
    }

    public void SetUp(DialogOption option)
    {
        this.option = option;
        text.text = option.russianText;
    }

    public void ShowUp()
    {
        text.enabled = true;
        image.enabled = true;
    }

    public void Click()
    {
        InteractionSystem.instance.ChangeToState(option);
    }
}
