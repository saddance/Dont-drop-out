using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimator : HumanAnimator
{
    private InteractableObject interactor;
    private HeroMotion hero;

    public override HumanAnimPData animData { 
        get
        {
            return interactor.personality.asHumanOnMap;
        } 
    }

    private void Awake()
    {
        interactor = GetComponent<InteractableObject>();
        hero = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroMotion>();
    }

    private void LookAtHero()
    {
        Vector3Int delta = new Vector3Int(
            Mathf.RoundToInt(hero.transform.position.x - transform.position.x),
            Mathf.RoundToInt(hero.transform.position.y - transform.position.y),
            0
            );
        SetLookingFromVector(delta, lookingState);
    }

    private bool IsTalkingWithHero()
    {
        var v = transform.position - hero.transform.position;
        return (Mathf.Abs(v.x - hero.lastDirection.x) <= 1e-3 &&
            Mathf.Abs(v.y - hero.lastDirection.y) <= 1e-3);
    }

    public void LateUpdate()
    {
        if (IsTalkingWithHero())
            LookAtHero();
        else
            lookingState = LookingState.front;
    }
}
