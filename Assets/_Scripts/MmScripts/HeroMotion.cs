using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Threading;

public class HeroMotion : MonoBehaviour
{
    [Header("Walking constants")]
    [SerializeField] private float moveTime;
    [SerializeField] private float comeBackTime;

    public Vector3Int lastDirection { get; private set; }
    private bool isMoving = false;
    private WASDHandler handler = new WASDHandler();

    void Update()
    {

        if (!isMoving)
        {

            // TODO : rewrite this shit
            var code = handler.GetPressedButton();
            if (code == null)
                return;
            else if (code == KeyCode.W)
                TryGo(new Vector3Int(0, 1, 0), code.Value);
            else if (code == KeyCode.A)
                TryGo(new Vector3Int(-1, 0, 0), code.Value);
            else if (code == KeyCode.S)
                TryGo(new Vector3Int(0, -1, 0), code.Value);
            else if (code == KeyCode.D)
                TryGo(new Vector3Int(1, 0, 0), code.Value);
        }
    }

    void TryGo(Vector3Int direction, KeyCode code)
    {
        lastDirection = direction;
        if (CanIGo())
        {
            isMoving = true;
            StartCoroutine(Walk(direction, code));
        }
    }

    IEnumerator Walk(Vector3 direction, KeyCode code)
    {
        float startTime = Time.time;
        Vector3 startPosition = transform.position;

        while (Time.time <= startTime + moveTime)
        {
            float elapsed = Time.time - startTime;
            if (elapsed <= comeBackTime && handler.GetPressedButton() != code)
            {
                direction = Vector3.zero;
                break;
            }
            transform.position = startPosition + (elapsed / moveTime) * direction;
            yield return new WaitForEndOfFrame();
        }

        transform.position = startPosition + direction;
        isMoving = false;
        yield break;
    }

    bool CanIGo()
    {
        try
        {
            return MapObjectManager.instance[Mathf.RoundToInt(transform.position.x) + lastDirection.x,
                Mathf.RoundToInt(transform.position.y) + lastDirection.y] == null;
        }
        catch (Exception)
        {
            return false;
        }
    }
}