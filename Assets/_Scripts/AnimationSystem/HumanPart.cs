using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HumanPart : MonoBehaviour
{
    protected HumanAnimator humanAnimator;
    protected HumanAnimPData animData;
    protected SpriteRenderer sRend;

    public virtual void Init(HumanAnimPData animData, HumanAnimator animatorParent)
    {
        sRend = GetComponent<SpriteRenderer>();
        humanAnimator = animatorParent;
        this.animData = animData;
    }
}
