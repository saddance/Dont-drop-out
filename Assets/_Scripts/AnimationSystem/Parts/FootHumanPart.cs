using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootHumanPart : HumanPart
{
    [SerializeField] private bool isLeft;

    [SerializeField] private float stepTime;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float sideStepUpAmplitude;
    [SerializeField] private float frontStepUpAmplitude;

    private Vector3 normalPosition;

    public override void Init(HumanAnimPData animData, HumanAnimator animatorParent)
    {
        base.Init(animData, animatorParent);
        normalPosition = transform.localPosition;
        sRend.color = animData.bootsColor.Get();
        StartCoroutine(SeekForWalking());
        StartCoroutine(SeekForSide());
    }

    IEnumerator SeekForSide()
    {
        while (true)
        {

            if (humanAnimator.lookingState == (isLeft ?
                HumanAnimator.LookingState.right : HumanAnimator.LookingState.left))
                transform.rotation = Quaternion.Euler(0, 180, 0);
            else
                transform.rotation = Quaternion.identity;

            if (humanAnimator.lookingState == HumanAnimator.LookingState.front)
                sRend.sortingOrder = Mathf.Abs(sRend.sortingOrder);
            else if (humanAnimator.lookingState == HumanAnimator.LookingState.back)
                sRend.sortingOrder = -Mathf.Abs(sRend.sortingOrder);
            else if (humanAnimator.lookingState == (isLeft ?
                HumanAnimator.LookingState.left : HumanAnimator.LookingState.right))
                sRend.sortingOrder = -Mathf.Abs(sRend.sortingOrder);
            else
                sRend.sortingOrder = Mathf.Abs(sRend.sortingOrder);

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator SeekForWalking()
    {
        while (true)
        {
            float p = 0;
            while (humanAnimator.animationState == HumanAnimator.AnimationState.walking)
            {
                p += Time.deltaTime / stepTime;
                UpdateMovingPosition(p);
                print("Walking");
                yield return new WaitForEndOfFrame();
            }

            MoveTo(normalPosition);
            yield return new WaitForEndOfFrame();
        }
    }

    private void MoveTo(Vector3 to)
    {
        float d = Mathf.Min((to - transform.localPosition).magnitude, Time.deltaTime * maxSpeed);
        transform.localPosition += d * (to - transform.localPosition).normalized;
    }

    private void UpdateMovingPosition(float p)
    {
        Vector3 destination = normalPosition;

        switch (humanAnimator.lookingState)
        {
            case HumanAnimator.LookingState.front:
            case HumanAnimator.LookingState.back:
                destination = new Vector3(
                    normalPosition.x,
                    normalPosition.y + frontStepUpAmplitude * Mathf.Max(0, Mathf.Sin(p * Mathf.PI +
                        (isLeft ? Mathf.PI : 0)))
                );
                break;
            case HumanAnimator.LookingState.left:
                destination = new Vector3(
                    normalPosition.x * Mathf.Cos(p * Mathf.PI),
                    normalPosition.y + sideStepUpAmplitude * Mathf.Max(0, Mathf.Sin(p * Mathf.PI +
                        (isLeft ? Mathf.PI : 0)))
                );
                break;
            case HumanAnimator.LookingState.right:
                destination = transform.localPosition = new Vector3(
                    normalPosition.x * Mathf.Cos(p * Mathf.PI),
                    normalPosition.y + sideStepUpAmplitude * Mathf.Max(0, Mathf.Sin(p * Mathf.PI +
                        (isLeft ? 0 : Mathf.PI)))
                );
                break;
        }
        MoveTo(destination);

    }
}