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
    [SerializeField] private float appearTime;
    [SerializeField] private float lettersSpeed;
    [SerializeField] private float optionsDelay;

    public bool IsReady { get; private set; } = false;

    private RectTransform rectTransform;
    private DialogState state;
    private Personality personality;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private bool WasSkip() =>
        Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return);
    

    void SetupLayout()
    {
        dialogName.text = personality.asDialog.personalityName;

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x,
            rectTransform.sizeDelta.y + state.options.Length *
            (layout.spacing + selectionButtonPrefab.GetComponent<RectTransform>().sizeDelta.y));

        StartCoroutine(Appear());
    }


    IEnumerator Appear()
    {
        float time = Time.time;
        bool immediatly = false;

        while (Time.time - time < appearTime)
        {
            immediatly |= WasSkip();
            if (immediatly)
                break;

            rectTransform.localScale = Vector3.one * Mathf.Min(1, (Time.time - time) / appearTime);
            yield return null;
        }

        rectTransform.localScale = Vector3.one;
        StartCoroutine(ShowText(immediatly));
    }

    IEnumerator ShowText(bool immediatly = false)
    {
        float time = Time.time;
        while (true)
        {
            immediatly |= WasSkip();
            if (immediatly)
                break;

            int first = Mathf.RoundToInt((Time.time - time) * lettersSpeed);
            if (first >= state.russianText.Length)
                break;
            mainText.text = state.russianText.Substring(0, first);

            yield return null;
        }
        mainText.text = state.russianText;
        StartCoroutine(ShowOptions(immediatly));
    }

    IEnumerator ShowOptions(bool immediatly = false)
    {
        float time = Time.time;
        foreach (var option in state.options)
        {
            time += optionsDelay;
            while (Time.time < time && !immediatly)
            {
                yield return null;
                immediatly = WasSkip();
            }

            var button = Instantiate(selectionButtonPrefab, layout.transform);
            button.SetUp(option);
        }
        
        IsReady = true;
    }

    public void Init(DialogState state, Personality personality)
    {
        this.state = state;
        this.personality = personality;
        SetupLayout();
    }



}
