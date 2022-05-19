using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirtHumanPart : HumanPart
{
    public override void Init(HumanAnimPData animData, HumanAnimator animatorParent)
    {
        base.Init(animData, animatorParent);
        sRend.color = animData.shirtColor.Get();
    }
}
