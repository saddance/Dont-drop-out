using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private TextAsset fieldText;
    [SerializeField] private GameObject prefabEnvironment;
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
            if (save.mapPositions[i] != null)
            {
                Sprite defaultSprite = null;
                if (save.personalities[i].asMapObject?.defaultSpriteName != null &&
                    save.personalities[i].asMapObject?.defaultSpriteName != "")
                    defaultSprite = Resources.Load<Sprite>($"Tiles/{save.personalities[i].asMapObject.defaultSpriteName}");

                MapObjectManager.instance.GenerateByPrefab(
                    prefabInteractable,
                    save.mapPositions[i].x,
                    save.mapPositions[i].y,
                    defaultSprite
                    );

                MapObjectManager.instance[save.mapPositions[i].x, save.mapPositions[i].y]
                    .GetComponent<InteractableObject>()
                    .Init(save.personalities[i]);
            }
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
                var availableSpawns = new List<Vector2Int>();
                for (int j = 0; j < spawnPositions.Count; j++)
                {
                    if (save.personalities[i].asMapObject.labels.Contains(j))
                        availableSpawns.AddRange(spawnPositions[j]);
                }

                if (availableSpawns.Count == 0)
                {
                    Debug.LogError($"Can't spawn personality {i}");
                    continue;
                }
                save.mapPositions[i] = availableSpawns[Random.Range(0, availableSpawns.Count)];

                for (int j = 0; j < spawnPositions.Count; j++)
                    if (spawnPositions[j].Contains(save.mapPositions[i].GetV()))
                        spawnPositions[j].Remove(save.mapPositions[i].GetV());
            }
        }
    }

    private List<List<Vector2Int>> ProcessFileAndSpawnObst()
    {
        var lines = fieldText.text.Split('\n');

        var height = lines.Length;
        var width = lines[0].Trim().Length;

        MapObjectManager.instance.MakeField(height, width/ 3);
        var spawnPositions = new List<List<Vector2Int>>();
        for (int i = 0; i < 16; i++)
            spawnPositions.Add(new List<Vector2Int>());

        for (var i = 0; i < height; i++)
            for (var j = 0; j < width; j += 3)
            {
                // X00 - XFF and SpaceSpaceSpace
                var hexNumber = lines[i].Substring(j, 3);
                var firstSymbol = hexNumber[0];
                if (firstSymbol != ' ')
                {
                    var index = char.IsDigit(firstSymbol) ? firstSymbol - '0' : 10 + firstSymbol - 'A';
                    spawnPositions[index].Add(new Vector2Int(j / 3, i));
                }

                var sprite = Resources.Load<Sprite>($"Tiles/{hexNumber.Substring(1, 2)}");

                MapObjectManager.instance.GenerateByPrefab(prefabEnvironment, i, j / 3, sprite, hexNumber[1] > '7');
            }

        return spawnPositions;
    }
}