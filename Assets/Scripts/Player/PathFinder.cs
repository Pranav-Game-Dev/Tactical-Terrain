using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PathFinder : MonoBehaviour
{
    private GridManager gridManager;
    private TileSelect tileSelect;
    private PlayerController playerController;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        tileSelect = FindObjectOfType<TileSelect>();
        playerController = FindObjectOfType<PlayerController>();

        if (gridManager == null || tileSelect == null || playerController == null)
        {
            Debug.LogError("Required components not found.");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (!gridManager.IsGridGenerated)
            return;

        if (tileSelect.SelectedTile == null || playerController.CurrentTile == null)
            return;

        if (tileSelect.SelectedTile == playerController.CurrentTile)
            return;

        GameObject selectedTile = tileSelect.SelectedTile;
        GameObject currentTile = playerController.CurrentTile;

        Vector2Int start = GetTilePosition(currentTile.name);
        Vector2Int target = GetTilePosition(selectedTile.name);

        List<Vector2Int> path = FindPath(start, target);

        if (path.Count > 0)
        {
            string pathString = string.Join(", ", path.Select(p => GetTileName(p)));
            Debug.Log($"Path: {{{pathString}}}");

            playerController.MovePlayerAlongPath(path);
        }
        else
        {
            Debug.Log("No path found.");
        }

        tileSelect.ResetSelectedTile();
    }

    private Vector2Int GetTilePosition(string tileName)
    {
        char row = tileName[0];
        int column = int.Parse(tileName.Substring(1));
        return new Vector2Int(column - 1, row - 'A');
    }

    private string GetTileName(Vector2Int position)
    {
        char row = (char)('A' + position.y);
        int column = position.x + 1;
        return $"{row}{column}";
    }

    private List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        HashSet<Vector2Int> blockedPositions = new HashSet<Vector2Int>();

        foreach (string blockedTileName in gridManager.BlockedTileArr)
        {
            blockedPositions.Add(GetTilePosition(blockedTileName));
        }

        Vector2Int current = start;
        while (current != target)
        {
            List<Vector2Int> neighbors = GetNeighbors(current);

            neighbors.Sort((a, b) => Vector2Int.Distance(a, target).CompareTo(Vector2Int.Distance(b, target)));
            neighbors.RemoveAll(pos => blockedPositions.Contains(pos));

            if (neighbors.Count > 0)
            {
                current = neighbors[0];
                path.Add(current);
            }
            else
            {
                Debug.LogWarning("No available path to the target.");
                break;
            }
        }

        return path;
    }

    private List<Vector2Int> GetNeighbors(Vector2Int position)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        Vector2Int[] possibleNeighbors = {
            new Vector2Int(position.x, position.y - 1),
            new Vector2Int(position.x, position.y + 1),
            new Vector2Int(position.x - 1, position.y),
            new Vector2Int(position.x + 1, position.y),
            new Vector2Int(position.x - 1, position.y - 1),
            new Vector2Int(position.x - 1, position.y + 1),
            new Vector2Int(position.x + 1, position.y - 1),
            new Vector2Int(position.x + 1, position.y + 1)
        };

        foreach (Vector2Int neighbor in possibleNeighbors)
        {
            if (IsValidTile(neighbor))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    private bool IsValidTile(Vector2Int position)
    {
        return position.x >= 0 && position.x < gridManager.gridSizeX && position.y >= 0 && position.y < gridManager.gridSizeZ;
    }
}