using UnityEngine;

public class MapObjectManager : MonoBehaviour
{
    public static MapObjectManager instance;
    private GameObject[,] gridArray;

    public Vector2Int Length => new Vector2Int(gridArray.GetLength(0), gridArray.GetLength(1));

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

    public void GenerateByPrefab(GameObject prefab, int x, int y, Sprite sprite = null, bool setOnMap = true)
    {
        if (prefab == null)
        {
            Debug.LogError("null prefab tried to be spawned");
            return;
        }
        var obj = Instantiate(prefab, transform);
        obj.transform.name = $"object at ({x}, {y})";
        obj.transform.position = new Vector3(x, y, 0);
        if (sprite != null)
            obj.GetComponent<SpriteRenderer>().sprite = sprite;
        if (setOnMap)
            this[x, y] = obj.gameObject;
    }
}