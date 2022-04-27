using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private TextAsset fieldText;
    [SerializeField] GameObject prefabObstacle;
    [SerializeField] GameObject prefabInteractable;

    private void InitPersonalityMap()
    {
        var save = GameManager.currentSave;

        for (int i = 0; i < save.mapPositions.Length; i++)
        {
            if (save.personalities[i].hidden)
                continue;

            MapObjectManager.instance.GenerateByPrefab(
                prefabInteractable,
                save.mapPositions[i].x,
                save.mapPositions[i].y);

            MapObjectManager.instance[save.mapPositions[i].x, save.mapPositions[i].y]
                .GetComponent<InteractableObject>()
                .Init(save.personalities[i]);
        }
    }

    private void CheckForPositionInit(List<Vector2Int> spawnPositions)
    {
        var save = GameManager.currentSave;

        if (save.mapPositions == null)
        {
            save.mapPositions = new Vector2IntS[save.personalities.Length];
            for (int i = 0; i < save.mapPositions.Length; i++)
            {
                if (save.personalities[i].hidden)
                    continue;
                save.mapPositions[i] = spawnPositions[Random.Range(0, spawnPositions.Count)];
                spawnPositions.Remove(save.mapPositions[i].GetV());
            }
        }
    }

    private List<Vector2Int> ProcessFileAndSpawnObst()
    {
        var lines = fieldText.text.Split('\n');

        var height = lines.Length;
        var width = lines[0].Trim().Length;

        MapObjectManager.instance.MakeField(width, height);
        List<Vector2Int> spawnPositions = new List<Vector2Int>();

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var symbol = lines[i][j];

                if (symbol == '1')
                    MapObjectManager.instance.GenerateByPrefab(prefabObstacle, j, i);
                else if (symbol == '2')
                    spawnPositions.Add(new Vector2Int(j, i));
            }
        }

        return spawnPositions;
    }

    void Start()
    {
        CheckForPositionInit(ProcessFileAndSpawnObst());
        InitPersonalityMap();
    }
}
