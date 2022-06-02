using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadHumanPart : HumanPart
{
    public override void Init(HumanAnimPData animData, HumanAnimator animatorParent)
    {
        base.Init(animData, animatorParent);
        transform.localPosition += animData.headDelta.Get();
    }
}
