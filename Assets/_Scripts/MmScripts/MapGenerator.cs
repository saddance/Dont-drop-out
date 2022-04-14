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
        var width = firstLine[0];
        var height = firstLine[1];

        MapObjectManager.instance.MakeField(height, width);
        for (int i = 1; i < height + 1; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var symbol = lines[i][j];
                MapObjectManager.instance.GenerateByPrefab(GetPrefabFromSymbol(symbol), j, i-1);
            }
        }
    }

    public GameObject GetPrefabFromSymbol(char symbol)
    {
        if (symbol == '1')
            return MapObjectManager.instance.PrefabObstacle;
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
