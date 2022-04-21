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

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        if (Input.GetKeyDown("w"))
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
        if (Input.GetKeyDown("s"))
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
        if (Input.GetKeyDown("d"))
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
        if (Input.GetKeyDown("a"))
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
    }

    bool CanIGo()
    {
        return MapObjectManager.instance[(int) transform.position.x + direction.x,
            (int) transform.position.y + direction.y] == null;
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
