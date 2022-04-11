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

    /*public enum FieldType
    {
        Empty,
        Wall
    }*/
    public void CreateFromFile()
    {
        var text = File.ReadAllLines(@"D:\Users\silae\Projects\Dont-drop-out\Dont-drop-out\Assets\TextAsset.txt");

        var firstLine = text[0]
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
                var symbol = text[i][j];
                MapObjectManager.instance.GenerateByPrefab(GetPrefabFromSymbol(symbol), j, i-1);
            }
        }
    }

    public GameObject GetPrefabFromSymbol(char symbol)
    {
        //можно сделать где - нибудь словарь <char, GameObject> или <enum, GameObject>
        if (symbol == '1')
            return MapObjectManager.instance.PrefabObstacle;
        return null;
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        CreateFromFile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
