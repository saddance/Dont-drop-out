using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapObjectManager : MonoBehaviour
{
	public static MapObjectManager instance = null;
	private GameObject[,] gridArray;
	
	public GameObject PrefabObstacle;
	public GameObject Banana;

	public GameObject this[int x, int y]
	{
		get { return gridArray[x, y]; }
		set
		{
			if (value != null && gridArray[x, y] != null)
				Debug.LogError("InvalidIndexes");
			else
				gridArray[x, y] = value;
		}
	}

	public void MakeField(int height, int width)
	{
		gridArray = new GameObject[height, width];
	}

	void Awake()
	{
		if (instance == null)
			instance = this;
	}

	public void GenerateByPrefab(GameObject prefab, int x, int y)
	{
		if (prefab == null)
			return;
		var obj = Instantiate(prefab);
		obj.transform.position = new Vector3(x, y, 0);
		this[x, y] = obj;
	}

	void Start()
	{

	}

	void Update()
	{

	}
}

