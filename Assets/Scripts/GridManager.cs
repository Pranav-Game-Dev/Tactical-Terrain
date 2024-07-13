using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject blockedTilePrefab1;
    public GameObject blockedTilePrefab2;
    public int gridSizeX = 10;
    public int gridSizeZ = 10;
    public float tilePlacementDelay = 0.2f;

    public bool IsGridGenerated { get; private set; } = false;
    public List<string> BlockedTileArr { get; private set; } = new List<string>();

    private GameObject[,] gridTiles;

    public event Action OnGridGenerated;

    void Start()
    {
        gridTiles = new GameObject[gridSizeX, gridSizeZ];
        StartCoroutine(GenerateGridWithObstacles());
    }

    private IEnumerator GenerateGridWithObstacles()
    {
        Transform[] parentObjects = new Transform[gridSizeZ];
        for (int i = 0; i < gridSizeZ; i++)
        {
            GameObject parent = new GameObject(((char)('A' + i)).ToString());
            parent.transform.SetParent(transform);
            parentObjects[i] = parent.transform;
        }

        int blockedTilesCount = UnityEngine.Random.Range(20, 26);
        int placedBlockedTiles = 0;

        for (int z = 0; z < gridSizeZ; z++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                bool isBlockedTile = placedBlockedTiles < blockedTilesCount && UnityEngine.Random.value < (float)(blockedTilesCount - placedBlockedTiles) / (gridSizeX * gridSizeZ - (z * gridSizeX + x));
                GameObject tile;

                if (isBlockedTile)
                {
                    tile = Instantiate(UnityEngine.Random.value < 0.5f ? blockedTilePrefab1 : blockedTilePrefab2, new Vector3(x + 1, 3, z + 1), Quaternion.identity);
                    tile.name = $"Block{BlockedTileArr.Count + 1}";
                    BlockedTileArr.Add($"{(char)('A' + z)}{x + 1}");
                    placedBlockedTiles++;
                }
                else
                {
                    tile = Instantiate(tilePrefab, new Vector3(x + 1, 3, z + 1), Quaternion.identity);
                    tile.name = $"{(char)('A' + z)}{x + 1}";
                }

                tile.transform.SetParent(parentObjects[z]);
                StartCoroutine(MoveTileToPosition(tile, new Vector3(x + 1, 1, z + 1)));
                gridTiles[x, z] = tile;

                yield return new WaitForSeconds(tilePlacementDelay);
            }
        }

        Debug.Log($"Placed {placedBlockedTiles} blocked tiles.");
        IsGridGenerated = true;
        OnGridGenerated?.Invoke();
    }

    private IEnumerator MoveTileToPosition(GameObject tile, Vector3 targetPosition)
    {
        while (tile.transform.position != targetPosition)
        {
            tile.transform.position = Vector3.MoveTowards(tile.transform.position, targetPosition, Time.deltaTime * 5);
            yield return null;
        }
    }

    public Vector3 GetWorldPosition(Vector2Int gridPosition)
    {
        if (IsValidTile(gridPosition))
        {
            return gridTiles[gridPosition.x, gridPosition.y].transform.position;
        }
        return Vector3.zero;
    }

    public GameObject GetTileAtPosition(Vector3 worldPosition)
    {
        for (int z = 0; z < gridSizeZ; z++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                if (gridTiles[x, z] != null && Vector3.Distance(gridTiles[x, z].transform.position, worldPosition) < 0.1f)
                {
                    return gridTiles[x, z];
                }
            }
        }
        return null;
    }

    private bool IsValidTile(Vector2Int gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < gridSizeX && gridPosition.y >= 0 && gridPosition.y < gridSizeZ;
    }
}