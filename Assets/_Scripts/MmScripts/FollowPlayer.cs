using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}
