using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectManager : MonoBehaviour
{
    public static MapObjectManager instance = null;
    private GameObject[,] gridArray;
    [SerializeField]
    private GameObject prefabObstacle;
    public GameObject this[int x, int y]
    {
        get
        {
            return gridArray[x, y];
        }
        set
        {
            if (value != null && gridArray[x, y] != null)
                Debug.LogError("InvalidIndexes");
            else 
                gridArray[x, y] = value;
        }
    }


    void Awake()
    {
        if (instance == null)
            instance = this;
        gridArray = new GameObject[10, 10];
    }

    void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            var a = Instantiate(prefabObstacle, new Vector3(i, 0, 0), Quaternion.identity);
            gridArray[i, 0] = a;
            if (i != 0)
            {
                var b = Instantiate(prefabObstacle, new Vector3(0, i, 0), Quaternion.identity);
                gridArray[0, i] = b;
            }
        }
    }
}
