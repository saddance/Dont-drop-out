using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionButton : MonoBehaviour
{
    [SerializeField] float additionalSize;
    [SerializeField] Color inactiveColor = new Color(0.5f, 0.5f, 0.5f); 
    Text text;
    Image image;
    DialogOption option;
    Button button;

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
        image = GetComponentInChildren<Image>();
        button = GetComponentInChildren<Button>();
    }

    public void SetUp(DialogOption option, Personality personality)
    {
        this.option = option;
        if (option.IsActive(personality))
        {
            text.text = option.russianText;
            button.interactable = true;
        }
        else
        {
            text.text = option.russianInactiveText;
            button.interactable = false;
            image.color = inactiveColor;
        }
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
