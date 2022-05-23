using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class InteractionSystem : MonoBehaviour
{
    public static InteractionSystem instance;
    public bool OnDialog { get; private set; } = false;
    private Personality _personality;
    private DialogState currentState;
    private DialogPanel dialogPanel;
    [SerializeField] private DialogPanel panelPrefab;


    private void Awake()
    {
        instance = this; 
    }

    private DialogStart GetBestStart()
    {
        var starts = _personality.asDialog.availableDialogStarts;
        return starts
            .OrderBy(x => x)
            .Where(x => x.CanBeUsed())
            .FirstOrDefault();
    }

    private DialogState LoadState(string prefix)
    {
        var states = Resources
            .LoadAll<DialogState>("Dialogs")
            .Where(x => x.name.StartsWith(prefix))
            .ToList();
        return states[Random.Range(0, states.Count)];
    }

    public void StartInteraction(Personality personality)
    {
        if (personality.asDialog == null)
            return;

        _personality = personality;
        var bestStart = GetBestStart();
        if (bestStart == null)
            return;

        OnDialog = true;
        ChangeToState(new DialogOption() { 
            nextDialogPrefix = bestStart.dialogPrefix, 
            option = DialogOption.OptionType.nextDialog });
    }

    private void ShowDialogState()
    {
        if (dialogPanel != null)
            Destroy(dialogPanel.gameObject);
        dialogPanel = Instantiate(panelPrefab, transform);
        dialogPanel.Init(currentState, _personality);
    }


    private void EndInteraction()
    {
        if (dialogPanel != null)
            Destroy(dialogPanel.gameObject);
        OnDialog = false;
    }

    public void ChangeToState(DialogOption option)
    {
        if (!OnDialog)
            throw new System.Exception("Can't change state while on dialog!");

        switch (option.option)
        {
            case DialogOption.OptionType.nextDialog:
                currentState = LoadState(option.nextDialogPrefix);
                ShowDialogState();
                break;
            case DialogOption.OptionType.quit:
                EndInteraction();
                break;
            case DialogOption.OptionType.startBattle:
                GameManager.StartBattle(_personality);
                break;
            default:
                break;
        }
    }

}
