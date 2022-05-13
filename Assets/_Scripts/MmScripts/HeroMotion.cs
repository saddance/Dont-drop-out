using System;
using System.Collections;
using UnityEngine;

public class HeroMotion : MonoBehaviour
{
    [Header("Walking constants")] [SerializeField]
    private float moveTime;

    [SerializeField] private float comeBackTime;
    public bool Pause;
    private readonly WASDHandler handler = new WASDHandler();
    public bool isMoving;

    public Vector3Int lastDirection { get; private set; }

    private void Start()
    {
        var pos = GameManager.currentSave.playerPosition;
        transform.position = new Vector3(pos.x, pos.y);
    }

    private void Update()
    {
        if (Pause)
            return;
        
        if (Input.GetKeyDown(KeyCode.P)) GameManager.SaveGame();
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            try
            {
                var objAhead = MapObjectManager.instance[Mathf.RoundToInt(transform.position.x) + lastDirection.x,
                    Mathf.RoundToInt(transform.position.y) + lastDirection.y];

                var component = objAhead.GetComponent<InteractableObject>();
                GameManager.StartBattle(component.personality);
                return;
            }
            catch (Exception)
            {
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            
        }

        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Pause)
                    Pause = false;
                else
                    GameManager.ExitGame();
            }

            // TODO : rewrite this shit
            var code = handler.GetPressedButton();
            if (code == null)
                return;
            if (code == KeyCode.W)
                TryGo(new Vector3Int(0, 1, 0), code.Value);
            else if (code == KeyCode.A)
                TryGo(new Vector3Int(-1, 0, 0), code.Value);
            else if (code == KeyCode.S)
                TryGo(new Vector3Int(0, -1, 0), code.Value);
            else if (code == KeyCode.D)
                TryGo(new Vector3Int(1, 0, 0), code.Value);
        }
    }

    private void TryGo(Vector3Int direction, KeyCode code)
    {
        lastDirection = direction;
        if (CanIGo())
        {
            isMoving = true;
            StartCoroutine(Walk(direction, code));
        }
    }

    private IEnumerator Walk(Vector3 direction, KeyCode code)
    {
        var startTime = Time.time;
        var startPosition = transform.position;

        while (Time.time <= startTime + moveTime)
        {
            var elapsed = Time.time - startTime;
            if (elapsed <= comeBackTime && handler.GetPressedButton() != code)
            {
                direction = Vector3.zero;
                break;
            }

            transform.position = startPosition + elapsed / moveTime * direction;
            yield return new WaitForEndOfFrame();
        }

        transform.position = startPosition + direction;
        isMoving = false;
    }

    private bool CanIGo()
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