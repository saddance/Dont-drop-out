using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HumanAnimator : MonoBehaviour
{
    public abstract HumanAnimPData animData { get; }

    public enum LookingState
    {
        front, 
        back,
        left,
        right
    }

    public enum AnimationState
    {
        idle,
        walking,
        attacking,
        defending,
        killed
    }

    public LookingState lookingState { get; protected set; }
    public AnimationState animationState { get; protected set; }

    protected void SetLookingFromVector(Vector3Int delta, LookingState defaultState)
    {
        if (delta == Vector3Int.down)
            lookingState = LookingState.front;
        else if (delta == Vector3Int.up)
            lookingState = LookingState.back;
        else if (delta == Vector3Int.right)
            lookingState = LookingState.right;
        else if (delta == Vector3Int.left)
            lookingState = LookingState.left;
        else
            lookingState = defaultState;
    }


    private void Start()
    {
        var parts = GetComponentsInChildren<HumanPart>();
        foreach (var part in parts)
            part.Init(animData, this);
    }
}
