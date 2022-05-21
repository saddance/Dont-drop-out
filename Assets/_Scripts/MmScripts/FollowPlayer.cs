using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float minDistance = 0.05f;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        var pos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        if (GameManager.currentSave != null)
            GameManager.currentSave.playerPosition = pos;
    }

    private void LateUpdate()
    {
        var dest = new Vector3(player.position.x, player.position.y, transform.position.z);
        var delta = (dest - transform.position);
        transform.position += delta * (Mathf.Max(0, delta.magnitude - minDistance) / delta.magnitude); 
    }
}