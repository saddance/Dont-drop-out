using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class InteractionSystem : MonoBehaviour
{
    public static InteractionSystem instance;
    public bool OnDialog { get; private set; } = false;
    private Personality _personality;
    private DialogState currentState;

    private void Awake()
    {
        instance = this; 
    }

    public void StartInteraction(Personality personality)
    {
        if (personality.asDialog == null)
            return;

        OnDialog = true;
        _personality = personality;
        print("Dialog starts!");
    }

}
