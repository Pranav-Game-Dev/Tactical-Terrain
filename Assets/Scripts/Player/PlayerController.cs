using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public GameObject playerPrefab;
    public float movementDuration = 0.5f;
    public float jumpHeight = 3f;
    public float positionOffset = 1.8f;

    public bool isPlayerMoving = false;
    public GameObject CurrentTile { get; private set; }

    private GridManager gridManager;
    private PathFinder pathFinder;
    private GameObject playerObject;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
        StartCoroutine(InitializePlayer());
    }

    IEnumerator InitializePlayer()
    {
        yield return new WaitUntil(() => gridManager.IsGridGenerated);

        List<string> availableTiles = new List<string>{"A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "A10"};
        availableTiles.RemoveAll(tile => gridManager.BlockedTileArr.Contains(tile));

        if (availableTiles.Count > 0)
        {
            string initialTile = availableTiles[Random.Range(0, availableTiles.Count)];
            CurrentTile = GameObject.Find(initialTile);
            Vector3 spawnPosition = CurrentTile.transform.position + Vector3.up + Vector3.right * positionOffset;
            playerObject = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("No available tiles for player spawn.");
        }
    }

    public void MovePlayerAlongPath(List<Vector2Int> path)
    {
        if (path.Count > 0 && !isPlayerMoving)
        {
            StartCoroutine(MoveAlongPath(path));
        }
    }

    IEnumerator MoveAlongPath(List<Vector2Int> path)
    {
        isPlayerMoving = true;

        foreach (Vector2Int tilePos in path)
        {
            string tileName = $"{(char)('A' + tilePos.y)}{tilePos.x + 1}";
            GameObject targetTile = GameObject.Find(tileName);

            if (targetTile != null)
            {
                Vector3 startPos = playerObject.transform.position;
                Vector3 endPos = targetTile.transform.position + Vector3.up + Vector3.right * positionOffset;
                Vector3 controlPoint = (startPos + endPos) / 2 + Vector3.up * jumpHeight;

                float elapsedTime = 0f;
                while (elapsedTime < movementDuration)
                {
                    float t = elapsedTime / movementDuration;
                    Vector3 m1 = Vector3.Lerp(startPos, controlPoint, t);
                    Vector3 m2 = Vector3.Lerp(controlPoint, endPos, t);
                    playerObject.transform.position = Vector3.Lerp(m1, m2, t);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                playerObject.transform.position = endPos;
                CurrentTile = targetTile;
            }
        }

        isPlayerMoving = false;
    }
}