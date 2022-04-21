using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance = null;
    [SerializeField] private TextAsset fieldText;

    public void CreateFromFile()
    {
        var lines = fieldText.text.Split('\n');

        var firstLine = lines[0]
            .Split(' ')
            .Select(int.Parse)
            .ToList();
        var height = lines.Length;
        var width = lines[0].Length;

        MapObjectManager.instance.MakeField(height, width);
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var symbol = lines[i][j];
                MapObjectManager.instance.GenerateByPrefab(GetPrefabFromSymbol(symbol), j, i);
            }
        }
    }

    public GameObject GetPrefabFromSymbol(char symbol)
    {
        if (symbol == '1')
            return MapObjectManager.instance.PrefabObstacle;
        if (symbol == '2')
            return MapObjectManager.instance.Banana;
        return null;
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    
    void Start()
    {
        CreateFromFile();
    }
}
