using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    public static InteractionSystem instance;

    private void Awake()
    {
        instance = this; 
    }

    public void StartInteraction(Personality personality)
    {
        if (personality == null)
            return;

        print("Dialog starts!");
    }

}
