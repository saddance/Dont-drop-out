using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Threading;

public class HeroMotionManager : MonoBehaviour
{
    public Vector3Int direction;
    public bool Pause;

    void Update()
    {
        if (Pause)
            return;
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        if (Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction.x = 0;
            direction.y = 1;
            if (CanIGo())
            {
                Moving();
            }
        }
        if (vertical == 1)
        {
            direction.x = 0;
            direction.y = 1;
            if (CanIGo())
            {
                Running();
            }
        }
        if (Input.GetKeyDown("s") || Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction.x = 0;
            direction.y = -1;
            if (CanIGo())
            {
                Moving();
            }
        }
        if (vertical == -1)
        {
            direction.x = 0;
            direction.y = -1;
            if (CanIGo())
            {
                Running();
            }
        }
        if (Input.GetKeyDown("d") || Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction.x = 1;
            direction.y = 0;
            if (CanIGo())
            {
                Moving();
            }
        }
        if (horizontal == 1)
        {
            direction.x = 1;
            direction.y = 0;
            if (CanIGo())
            {
                Running();
            }
        }
        if (Input.GetKeyDown("a") || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction.x = -1;
            direction.y = 0;
            if (CanIGo())
            {
                Moving();
            }
        }
        if (horizontal == -1)
        {
            direction.x = -1;
            direction.y = 0;
            if (CanIGo())
            {
                Running();
            }
        }

        if (Input.GetKeyDown("e"))
        {
            var position = transform.position;
            var x = position.x + direction.x;
            var y = position.y + direction.y;
            var objAhead = MapObjectManager.instance[(int)transform.position.x + direction.x,
                (int)transform.position.y + direction.y];
            var component = objAhead.GetComponent<InteractableObject>();
            if (component != null)
            {
                Pause = true;
                print("Interaction starts");
            }
        }
    }

    bool CanIGo()
    {
        try
        {
            return MapObjectManager.instance[(int)transform.position.x + direction.x,
                (int)transform.position.y + direction.y] == null;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    void Moving()
    {
        transform.position += direction;
        Thread.Sleep(160);
    }

    void Running()
    {
        transform.position += direction;
        Thread.Sleep(100);
    }
}