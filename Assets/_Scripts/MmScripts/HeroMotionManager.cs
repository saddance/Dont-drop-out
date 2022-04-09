using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HeroMotionManager : MonoBehaviour
{
    public float speed = 0.05f;

    public Vector2Int direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal") * speed;
        var vertical = Input.GetAxis("Vertical") * speed;
        var v = new Vector3(horizontal, vertical, 0);
        SetDirection(horizontal, vertical);
        var x = (int) Math.Round(transform.position.x);
        var y = (int) Math.Round(transform.position.y);
        if (MapObjectManager.instance[x + direction.x, y + direction.y] == null)
        {
            transform.position += v;
        }

    }

    private void SetDirection(float horizontal, float vertical)
    {
        if (horizontal > 0)
        {
            if (vertical == 0)
            {
                direction.y = 0;
            }
            direction.x = 1;
        }
        if (horizontal < 0)
        {
            if (vertical == 0)
            {
                direction.y = 0;
            }
            direction.x = -1;
        }
        if (vertical > 0)
        {
            if (horizontal == 0)
            {
                direction.x = 0;
            }
            direction.y = 1;
        }
        if (vertical < 0)
        {
            if (horizontal == 0)
            {
                direction.x = 0;
            }
            direction.y = -1;
        }
    }
}
