using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class InteractionSystem : MonoBehaviour
{
    public static InteractionSystem instance;
    public bool OnDialog { get; private set; } = false;
    public Personality personality { get; private set; }
    private DialogState currentState;
    private DialogPanel dialogPanel;
    [SerializeField] private DialogPanel panelPrefab;

    private DialogState[] states;

    private void Awake()
    {
        instance = this;
        states = Resources.LoadAll<DialogState>("Dialogs");
    }

    private DialogPData DialogInfo => personality.asDialog;
    private int CurrentDay => GameManager.currentSave.currentDay;

    private DialogState[] GetAvailableStates(string prefix = "") => states
        .Where(x => x.name.StartsWith(prefix) && x.IsActive(personality))
        .ToArray();


    private IEnumerable<DialogStart> GetAvailableSpecialStarts() => DialogInfo.uniqueDialogStarts
            .Where(x => x.lastDayUsed == -1)
            .Concat(DialogInfo.dailyDialogStarts
                    .Where(x => x.lastDayUsed < CurrentDay)
                    )
            .Where(x => GetAvailableStates(x.dialogPrefix).Any());

    private IEnumerable<DialogStart> GetAvailableCommonStarts() => DialogInfo.commonDialogStarts
        .Where(x => GetAvailableStates(x.dialogPrefix).Any());

    private DialogStart SelectRandom(IEnumerable<DialogStart> dialogStarts)
    {
        var arr = dialogStarts.ToArray();
        if (arr.Length == 0)
            return null;
        return arr[Random.Range(0, arr.Length)];
    }

    private DialogStart GetBestStart()
    {
        var dialogInfo = personality.asDialog;

        var result = SelectRandom(GetAvailableSpecialStarts()
            .Where(x => x.importance == DialogStart.Importance.MustBeShown));
        if (result != null)
            return result;

        if (DialogInfo.lastDayUsed != CurrentDay)
        {
            result = SelectRandom(GetAvailableSpecialStarts());
            if (result != null)
                return result;
        }

        result = SelectRandom(GetAvailableCommonStarts()
            .Where(x => x.importance == DialogStart.Importance.MustBeShown));
        if (result != null)
            return result;

        return SelectRandom(GetAvailableCommonStarts());
    }

    private DialogState LoadState(string prefix)
    {
        var states = GetAvailableStates(prefix);
        return states[Random.Range(0, states.Length)];
    }

    public void StartInteraction(Personality personality)
    {
        if (personality.asDialog == null)
            return;

        this.personality = personality;
        var bestStart = GetBestStart();
        if (bestStart == null)
            return;

        OnDialog = true;

        this.personality.asDialog.lastDayUsed = GameManager.currentSave.currentDay;
        bestStart.lastDayUsed = GameManager.currentSave.currentDay;

        ChangeToState(new DialogOption() { 
            nextDialogPrefix = bestStart.dialogPrefix, 
            option = DialogOption.OptionType.nextDialog,
            effects = new DialogEffects() });
    }

    private void ShowDialogState()
    {
        if (dialogPanel != null)
            Destroy(dialogPanel.gameObject);
        dialogPanel = Instantiate(panelPrefab, transform);
        dialogPanel.Init(currentState, personality);
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

        option.effects.Effect(personality);

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
                GameManager.StartBattle(personality);
                break;
            case DialogOption.OptionType.givePresent:
                var nxt = new DialogOption()
                {
                    nextDialogPrefix = option.nextDialogPrefix,
                    option = DialogOption.OptionType.nextDialog,
                    effects = new DialogEffects()
                };
                InventoryStarter.instance.ShowInventoryForGift("present", x => {
                    if (x == -1)
                        EndInteraction();
                    else
                    {
                        personality.asFriend.friendScore += InventoryMaster.ItemAt(x).friendshipAffect;
                        InventoryMaster.At(x).amount--;
                        ChangeToState(nxt);
                    }
                    });
                break;
            default:
                break;
        }
    }

}
