using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesHumanPart : HumanPart
{
    [SerializeField] private bool isLeft;
    [SerializeField] private float headBorder = 0.34f;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float nearbyMult;

    private bool onFront
    {
        get => sRend.sortingOrder > 0;
        set
        {
            sRend.sortingOrder = (value ? 1 : -1) * Mathf.Abs(sRend.sortingOrder);
        }
    }
    

    Vector3 normalPosition;

    public override void Init(HumanAnimPData animData, HumanAnimator animatorParent)
    {
        base.Init(animData, animatorParent);
        transform.localPosition = animData.eyePosition.Get();
        if (isLeft)
            transform.localPosition = new Vector3(-transform.localPosition.x, 
                transform.localPosition.y);
        transform.localScale = animData.eyeScale.Get();

        normalPosition = transform.localPosition;
        StartCoroutine(SeekForRotation());
    }

    IEnumerator SeekForRotation()
    {
        while (true)
        {
            Vector3 togo = normalPosition;
            bool isFront = true;

            switch (humanAnimator.lookingState)
            {
                case HumanAnimator.LookingState.front:
                    break;
                case HumanAnimator.LookingState.back:
                    isFront = false;
                    togo = new Vector3(
                        -normalPosition.x,
                        normalPosition.y
                        );
                    break;
                case HumanAnimator.LookingState.left:
                    togo = new Vector3(
                        -headBorder + (isLeft ? 0 : normalPosition.x * nearbyMult),
                        normalPosition.y
                        );
                    break;
                case HumanAnimator.LookingState.right:
                    togo = new Vector3(
                        headBorder + (isLeft ? normalPosition.x * nearbyMult : 0),
                        normalPosition.y
                        );
                    break;
            }
            MoveTo(togo, isFront);

            yield return new WaitForEndOfFrame();

        }

    }

    private void MoveTo(Vector3 to, bool isFront)
    {
        if (isFront == onFront)
        {
            float d = Mathf.Min((to - transform.localPosition).magnitude, Time.deltaTime * maxSpeed);
            transform.localPosition += d * (to - transform.localPosition).normalized;
        }
        else
        {
            Vector3 dest;
            if (isFront)
            {
                if (to.x > normalPosition.x + 1e-6)
                    dest = new Vector3(headBorder, normalPosition.y);
                else
                    dest = new Vector3(-headBorder, normalPosition.y);
            }
            else
            {
                if (transform.localPosition.x < normalPosition.x - 1e-6)
                    dest = new Vector3(-headBorder, normalPosition.y);
                else
                    dest = new Vector3(headBorder, normalPosition.y);
            }
            if ((dest - transform.localPosition).magnitude < 1e-6)
            {
                onFront = !onFront;
                MoveTo(to, isFront);
            }
            else
                MoveTo(dest, onFront);
        }
    }
}
