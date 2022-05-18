using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;

namespace _Scripts.MmScripts
{
    // AsMapObject пока не используется 
    [Serializable]
    public class AsMapObject
    {
        public List<int> Labels;
    }
    public class MathMech : MonoBehaviour
    {
        
    [SerializeField] private TextAsset fieldText;
    //[SerializeField] private GameObject prefabObstacle;
    [SerializeField] private GameObject prefabInteractable;

    private void Start()
    {
        CheckForPositionInit(ProcessFileAndSpawnObst());
        InitPersonalityMap();
    }

    private void InitPersonalityMap()
    {
        var save = GameManager.currentSave;

        for (var i = 0; i < save.mapPositions.Length; i++)
        {
            if (save.personalities[i].hidden)
                continue;
            
            // Строчка ниже не подходит под то что спрайты разные у нас 
            // Отрефактори, Витя ^_^
            MapObjectManager.instance.GenerateByPrefab(
                prefabInteractable,
                save.mapPositions[i].x,
                save.mapPositions[i].y);
            
            MapObjectManager.instance[save.mapPositions[i].x, save.mapPositions[i].y]
                .GetComponent<InteractableObject>()
                .Init(save.personalities[i]);
        }
    }

    private void CheckForPositionInit(List<List<Vector2Int>> spawnPositions)
    {
        var save = GameManager.currentSave;

        if (save.mapPositions == null)
        {
            save.mapPositions = new Vector2IntS[save.personalities.Length];
            for (var i = 0; i < save.mapPositions.Length; i++)
            {
                if (save.personalities[i].hidden)
                    continue;
                var type = Random.Range(0, 16);
                save.mapPositions[i] = spawnPositions[type][Random.Range(0, spawnPositions[type].Count)];
                spawnPositions[type].Remove(save.mapPositions[i].GetV());
            }
        }
    }
    private List<List<Vector2Int>> ProcessFileAndSpawnObst()
    {
        var lines = fieldText.text.Split('\n');

        var height = lines.Length;
        var width = lines[0].Trim().Length;

        MapObjectManager.instance.MakeField(width, height);
        var spawnPositions = new List<List<Vector2Int>>(16);

        for (var i = 0; i < height; i++)
        for (var j = 0; j < width; j += 3)
        {
            // X00 - XFF and SpaceSpaceSpace
            var hexNumber = lines[i].Substring(j, 3);
            var firstSymbol = hexNumber[0];
            if (firstSymbol != ' ')
            {
                var index = Char.IsDigit(firstSymbol) ? firstSymbol - '0' : firstSymbol - 'A';
                spawnPositions[index].Add(new Vector2Int(j / 3, i));
                var pathToFile = "Sprites/sprite" + hexNumber.Substring(1, 2);
                var sprite = Resources.Load<Sprite>(pathToFile);
                var gameObj = new GameObject();
                SpriteRenderer rend = gameObj.AddComponent<SpriteRenderer>();
                rend.sprite = sprite;
                Instantiate(gameObj);
                gameObj.transform.position = new Vector3(j / 3, i, 0);
                if (hexNumber[1] <= '7')
                {
                }

                if (hexNumber[1] >= '8')
                {
                    MapObjectManager.instance[j / 3, i] = gameObj;
                }
            }
        }
        return spawnPositions;
    }
    }
}