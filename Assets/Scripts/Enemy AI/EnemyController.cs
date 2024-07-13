using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float movementDuration = 0.5f;
    public float jumpHeight = 3f;
    public float positionOffset = 1.8f;

    public bool isEnemyMoving = false;
    public GameObject EnemyCurrentTile { get; private set; }

    private GridManager gridManager;
    private PlayerController playerController;
    private GameObject enemyObject;
    private bool hasMovedFirstTime = false;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        playerController = FindObjectOfType<PlayerController>();
        StartCoroutine(InitializeEnemy());
    }

    IEnumerator InitializeEnemy()
    {
        yield return new WaitUntil(() => gridManager.IsGridGenerated);
        yield return new WaitForSeconds(1f); // 2-second delay

        List<string> availableTiles = new List<string>{"J1", "J2", "J3", "J4", "J5", "J6", "J7", "J8", "J9", "J10"};
        availableTiles.RemoveAll(tile => gridManager.BlockedTileArr.Contains(tile));

        if (availableTiles.Count > 0)
        {
            string initialTile = availableTiles[Random.Range(0, availableTiles.Count)];
            EnemyCurrentTile = GameObject.Find(initialTile);
            Vector3 spawnPosition = EnemyCurrentTile.transform.position + Vector3.up + Vector3.right * positionOffset;
            enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("No available tiles for enemy spawn.");
        }

        yield return new WaitUntil(() => playerController.isPlayerMoving);
        hasMovedFirstTime = true;
    }

    public void MoveEnemyAlongPath(List<Vector2Int> path)
    {
        if (path.Count > 0 && !isEnemyMoving && hasMovedFirstTime)
        {
            StartCoroutine(MoveAlongPath(path));
        }
    }

    IEnumerator MoveAlongPath(List<Vector2Int> path)
    {
        isEnemyMoving = true;

        foreach (Vector2Int tilePos in path)
        {
            string tileName = $"{(char)('A' + tilePos.y)}{tilePos.x + 1}";
            GameObject targetTile = GameObject.Find(tileName);

            if (targetTile != null)
            {
                Vector3 startPos = enemyObject.transform.position;
                Vector3 endPos = targetTile.transform.position + Vector3.up + Vector3.right * positionOffset;
                Vector3 controlPoint = (startPos + endPos) / 2 + Vector3.up * jumpHeight;

                float elapsedTime = 0f;
                while (elapsedTime < movementDuration)
                {
                    float t = elapsedTime / movementDuration;
                    Vector3 m1 = Vector3.Lerp(startPos, controlPoint, t);
                    Vector3 m2 = Vector3.Lerp(controlPoint, endPos, t);
                    enemyObject.transform.position = Vector3.Lerp(m1, m2, t);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                enemyObject.transform.position = endPos;
                EnemyCurrentTile = targetTile;
            }
        }

        isEnemyMoving = false;
    }
}