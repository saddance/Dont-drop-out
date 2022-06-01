using System;
using System.Collections;
using UnityEngine;

public class HeroMotion : MonoBehaviour
{
    [Header("Walking constants")] 
    [SerializeField] private float moveTime;
    [SerializeField] private float comeBackTime;

    private bool Pause
    {
        get => InteractionSystem.instance.OnDialog || InventoryStarter.instance.OnInventory || isMoving;
    }

    private readonly WASDHandler handler = new WASDHandler();
    public bool isMoving { get; private set; }

    public Vector3Int lastDirection { get; private set; }

    private void Start()
    {
        var pos = GameManager.currentSave.playerPosition;
        transform.position = new Vector3(pos.x, pos.y);
    }
    
    private Personality GetPersonalityAhead()
    {
        int x = Mathf.RoundToInt(transform.position.x) + lastDirection.x;
        int y = Mathf.RoundToInt(transform.position.y) + lastDirection.y;

        if (x < 0 || y < 0 || MapObjectManager.instance.Length.x <= x ||
            MapObjectManager.instance.Length.y <= y)
            return null;

        return MapObjectManager.instance[x,y]?.GetComponent<InteractableObject>()?.personality;
    }

    private void Update()
    {
        if (Pause)
            return;
        
        if (Input.GetKeyDown(KeyCode.P)) 
			GameManager.SaveGame();
        
		if (Input.GetKeyDown(KeyCode.Escape))
		    GameManager.ExitGame();

        if (Input.GetKeyDown(KeyCode.I))
            InventoryStarter.instance.ShowInventory();

        if (Input.GetKeyDown(KeyCode.E))
        {
            var personality = GetPersonalityAhead();
            if (personality != null)
            {
                InteractionSystem.instance.StartInteraction(personality);
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
            GameManager.currentSave.currentDay++;

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

    private void TryGo(Vector3Int direction, KeyCode code)
    {
        lastDirection = direction;
        if (CanIGo())
            StartCoroutine(Walk(direction, code));
        
    }

    private IEnumerator Walk(Vector3 direction, KeyCode code)
    {
        isMoving = true;

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