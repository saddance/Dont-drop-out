using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class HeroMotionManager : MonoBehaviour
{
    public float speed = 0.05f;

    public Vector2Int direction;
    // Start is called before the first frame update
    void Start()
    {
        transform.gameObject.tag = "Player";
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal") * speed;
        var vertical = Input.GetAxis("Vertical") * speed;
        var v = new Vector3(horizontal, vertical, 0);
        SetDirection(horizontal, vertical);
        if (v == Vector3.zero)
        {
            v.x = (float) (Math.Round(transform.position.x) - transform.position.x);
            v.y = (float) (Math.Round(transform.position.y) - transform.position.y);
        }
        if (CanIGo())
        {
            transform.position += v;
        }
    }

    private bool CanIGo()
    {
        if (direction.x < 0 && direction.y == 0)
        {
            return MapObjectManager.instance[(int) Math.Ceiling(transform.position.x) - 1,
                (int) Math.Round(transform.position.y)] == null;
        }
        if (direction.x > 0 && direction.y == 0)
        {
            return MapObjectManager.instance[(int) transform.position.x + 1,
                (int) Math.Round(transform.position.y)] == null;
        }
        if (direction.y < 0 && direction.x == 0)
        {
            return MapObjectManager.instance[(int) Math.Round(transform.position.x),
                (int) Math.Ceiling(transform.position.y) - 1] == null;
        }
        if (direction.y > 0 && direction.x == 0)
        {
            return MapObjectManager.instance[(int) Math.Round(transform.position.x),
                (int) transform.position.y + 1] == null;
        }
        if (direction.x < 0 && direction.y < 0)
        {
            return MapObjectManager.instance[(int) Math.Ceiling(transform.position.x) - 1,
                (int) Math.Ceiling(transform.position.y) - 1] == null;
        }
        if (direction.x < 0 && direction.y > 0)
        {
            return MapObjectManager.instance[(int) Math.Ceiling(transform.position.x) - 1,
                (int) transform.position.y + 1] == null;
        }
        if (direction.x > 0 && direction.y < 0)
        {
            return MapObjectManager.instance[(int) transform.position.x + 1,
                (int) Math.Ceiling(transform.position.y) - 1] == null;
        }
        if (direction.x > 0 && direction.y > 0)
        {
            return MapObjectManager.instance[(int) transform.position.x + 1,
                (int) transform.position.y + 1] == null;
        }
        return true;
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
