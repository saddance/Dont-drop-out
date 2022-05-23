using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : MonoBehaviour
{
    [SerializeField] private Text mainText;
    [SerializeField] private SelectionButton selectionButtonPrefab;
    [SerializeField] private Text dialogName;
    [SerializeField] private VerticalLayoutGroup layout;

    private RectTransform rectTransform;
    private DialogState state;
    private Personality personality;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void SetupLayout()
    {
        mainText.text = state.russianText;
        dialogName.text = personality.asDialog.personalityName;

        foreach (var option in state.options)
        {
            var button = Instantiate(selectionButtonPrefab, layout.transform);
            button.SetUp(option);
        }

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 
            rectTransform.sizeDelta.y + state.options.Length *
            (layout.spacing + selectionButtonPrefab.GetComponent<RectTransform>().sizeDelta.y));
    }

    public void Init(DialogState state, Personality personality)
    {
        this.state = state;
        this.personality = personality;
        SetupLayout();
    }



}
