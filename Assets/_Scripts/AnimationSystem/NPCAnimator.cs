using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimator : HumanAnimator
{
    private InteractableObject interactor;
    private GameObject hero;

    public override HumanAnimPData animData { 
        get
        {
            return interactor.personality.asHumanOnMap;
        } 
    }

    private void Awake()
    {
        interactor = GetComponent<InteractableObject>();
        hero = GameObject.FindGameObjectWithTag("Player");
    }

    public void LookAtHero()
    {
        Vector3Int delta = new Vector3Int(
            Mathf.RoundToInt(hero.transform.position.x - transform.position.x),
            Mathf.RoundToInt(hero.transform.position.y - transform.position.y),
            0
            );
        SetLookingFromVector(delta, lookingState);
    }

    public void LateUpdate()
    {
        if (InteractionSystem.instance.OnDialog)
            LookAtHero();
        else
            lookingState = LookingState.front;
    }
}
