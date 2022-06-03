using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPCAnimator : HumanAnimator
{
    private InteractableObject interactor;
    private HeroMotion hero;
    private LookingState defaultLooking;
    private bool _firstFrame = true;

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

    private void SetDefault()
    {
        var pos = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);

        Vector3Int[] poss = new Vector3Int[4]
        {
                    Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
        }.Where(v => MapObjectManager.instance[pos.x + v.x, pos.y + v.y] == null).ToArray();
        if (poss.Length > 0)
            SetLookingFromVector(poss[Random.Range(0, poss.Length)], defaultLooking);

        poss = new Vector3Int[4]
        {
            Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
        }.Where(v => MapObjectManager.instance[pos.x + v.x, pos.y + v.y]?.GetComponent<NPCAnimator>() != null).ToArray();
        if (poss.Length > 0)
            SetLookingFromVector(poss[Random.Range(0, poss.Length)], defaultLooking);

        defaultLooking = lookingState;
        _firstFrame = false;
    }

    public void LateUpdate()
    {
        if (_firstFrame)
            SetDefault();

        if (IsTalkingWithHero())
            LookAtHero();
        else
            lookingState = defaultLooking;
    }
}
