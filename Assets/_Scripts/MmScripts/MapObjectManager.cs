using UnityEngine;

public class MapObjectManager : MonoBehaviour
{
    public static MapObjectManager instance;
    private GameObject[,] gridArray;

    public GameObject this[int x, int y]
    {
        get => gridArray[x, y];
        set
        {
            if (value != null && gridArray[x, y] != null)
                Debug.LogError("InvalidIndexes");
            else
                gridArray[x, y] = value;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void MakeField(int height, int width)
    {
        gridArray = new GameObject[height, width];
    }

    public void GenerateByPrefab(GameObject prefab, int x, int y)
    {
        if (prefab == null)
            return;
        var obj = Instantiate(prefab);
        obj.transform.position = new Vector3(x, y, 0);
        this[x, y] = obj;
    }
}