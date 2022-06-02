using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    public static FollowPlayer instance;
    private Transform player;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }

    private void Update()
    {
        var pos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        if (GameManager.currentSave != null)
            GameManager.currentSave.playerPosition = pos;
    }

    public void MoveDelta(Vector3 delta, float timeToMove)
    {
        StartCoroutine(MoveEnum(delta, timeToMove));
    }

    IEnumerator MoveEnum(Vector3 delta, float moveTime)
    {
        var startTime = Time.time;
        var prevTime = Time.time;
        Vector3 passed = Vector3.zero;

        while (Time.time <= startTime + moveTime)
        {
            var elapsed = Time.time - prevTime;
            transform.position += elapsed / moveTime * delta;
            passed += elapsed / moveTime * delta;
            prevTime = Time.time;
            yield return new WaitForEndOfFrame();
        }

        transform.position += delta - passed;
    }

    //private void LateUpdate()
    //{
    //    var dest = new Vector3(player.position.x, player.position.y, transform.position.z);
    //    var delta = dest - transform.position;
    //    transform.position += delta; 
    //}
}