using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnimator : HumanAnimator
{
    public override HumanAnimPData animData => GameManager.currentSave.heroHumanAnim;

    private HeroMotion motion;

    private void Awake()
    {
        motion = gameObject.GetComponent<HeroMotion>();
    }

    private void LateUpdate()
    {
        if (motion.isMoving)
            animationState = AnimationState.walking;
        else
            animationState = AnimationState.idle;

        SetLookingFromVector(motion.lastDirection, LookingState.front);
    }
}
