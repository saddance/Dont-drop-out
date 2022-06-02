using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBattleAnimator : HumanAnimator
{
    private Unit parent;
    public override HumanAnimPData animData {
        get {
            if (parent == null)
                parent = GetComponentInParent<Unit>();
            return parent.Info.animData;
        } 
    }

    private void Update()
    {
        if (parent.Info.IsEnemysUnit)
            lookingState = LookingState.left;
        else 
            lookingState = LookingState.right;
    }

}
