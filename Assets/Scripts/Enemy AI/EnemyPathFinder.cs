using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyPathFinder : MonoBehaviour
{
    private GridManager gridManager;
    private PlayerController playerController;
    private EnemyController enemyController;
    private Vector2Int lastPlayerPosition;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        playerController = FindObjectOfType<PlayerController>();
        enemyController = FindObjectOfType<EnemyController>();
        lastPlayerPosition = Vector2Int.one * -1; // Invalid initial position
    }

    void Update()
    {
        if (!gridManager.IsGridGenerated || enemyController.isEnemyMoving || playerController.isPlayerMoving)
            return;

        if (playerController.CurrentTile == null || enemyController.EnemyCurrentTile == null)
            return;

        Vector2Int currentPlayerPosition = GetTilePosition(playerController.CurrentTile.name);

        // Only find a new path if the player has moved
        if (currentPlayerPosition != lastPlayerPosition)
        {
            Vector2Int start = GetTilePosition(enemyController.EnemyCurrentTile.name);
            List<Vector2Int> neighborTiles = GetNeighborTiles(currentPlayerPosition);
            Vector2Int target = GetNearestNeighborTile(start, neighborTiles);

            List<Vector2Int> path = FindPath(start, target);

            if (path.Count > 0)
            {
                string pathString = string.Join(", ", path.Select(p => GetTileName(p)));
                Debug.Log($"Enemy Path: {{{pathString}}}");

                enemyController.MoveEnemyAlongPath(path);
            }

            lastPlayerPosition = currentPlayerPosition;
        }
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

    private List<Vector2Int> GetNeighborTiles(Vector2Int position)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>
        {
            new Vector2Int(position.x, position.y - 1),
            new Vector2Int(position.x, position.y + 1),
            new Vector2Int(position.x - 1, position.y),
            new Vector2Int(position.x + 1, position.y)
        };

        return neighbors.Where(pos => IsValidTile(pos) && !gridManager.BlockedTileArr.Contains(GetTileName(pos))).ToList();
    }

    private Vector2Int GetNearestNeighborTile(Vector2Int start, List<Vector2Int> neighbors)
    {
        return neighbors.OrderBy(n => Vector2Int.Distance(start, n)).First();
    }

    private List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        HashSet<Vector2Int> blockedPositions = new HashSet<Vector2Int>(
            gridManager.BlockedTileArr.Select(t => GetTilePosition(t)));

        Vector2Int current = start;
        while (current != target)
        {
            List<Vector2Int> neighbors = GetNeighborTiles(current);
            neighbors.Sort((a, b) => Vector2Int.Distance(a, target).CompareTo(Vector2Int.Distance(b, target)));

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

    private bool IsValidTile(Vector2Int position)
    {
        return position.x >= 0 && position.x < gridManager.gridSizeX && 
               position.y >= 0 && position.y < gridManager.gridSizeZ;
    }
}