using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : MonoBehaviour
{
    [SerializeField] private float showAnimationDuration;
    [SerializeField] private float lettersSpawnSpeed;
    [SerializeField] private Text mainText;
    [SerializeField] private SelectionButton selectionButtonPrefab;
    private RectTransform rectTransform;
    private VerticalLayoutGroup layout;

    private DialogState state;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        layout = GetComponent<VerticalLayoutGroup>();
    }

    private void SetupLayout()
    {
        mainText.text = state.russianText;

        foreach (var option in state.options)
        {
            var button = Instantiate(selectionButtonPrefab, transform);
            button.SetUp(option);
        }

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 
            rectTransform.sizeDelta.y + state.options.Length *
            (layout.spacing + selectionButtonPrefab.GetComponent<RectTransform>().sizeDelta.y));
    }

    public void Init(DialogState state)
    {
        this.state = state;
        SetupLayout();
    }



}
