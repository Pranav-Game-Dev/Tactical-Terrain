using UnityEngine;
using System.Collections;

public class TileSelect : MonoBehaviour
{
    public GridManager gridManager; // Reference to the GridManager script
    public PlayerController playerController; // Reference to the PlayerController script
    public LayerMask tileLayer; // Layer mask to ensure raycast only hits tiles

    private GameObject selectedTile = null; // The currently selected tile
    private GameObject previousHoveredTile = null; // The previously hovered tile
    private float hoverHeight = 1.2f; // The height to move the tile when hovered
    private float hoverSpeed = 0.2f; // The speed of the hover animation (duration)

    public GameObject SelectedTile
    {
        get { return selectedTile; }
        private set
        {
            // If there was a previously selected tile, reset its position
            if (selectedTile != null)
            {
                StartCoroutine(ResetTilePosition(selectedTile));
            }
            selectedTile = value;
        }
    }
    
    public void ResetSelectedTile()
    {
        SelectedTile = null;
    }

    void Update()
    {
        // Check if the GridManager and PlayerController are assigned
        if (gridManager == null || playerController == null)
        {
            Debug.LogError("GridManager or PlayerController is not assigned.");
            return;
        }

        // Wait until the grid is fully generated
        if (!gridManager.IsGridGenerated) return;

        // If the player is moving, do not allow tile selection
        if (playerController.isPlayerMoving)
        {
            SelectedTile = null; // Reset the selected tile
            return;
        }

        // Perform a raycast to detect the tile under the mouse cursor
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Use the specified layer mask to ensure we only hit tiles
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayer))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Check if the object hit by the raycast is a tile and not a blocked tile
            if (!hitObject.name.StartsWith("Block"))
            {
                // Animate the tile on hover
                if (previousHoveredTile != hitObject)
                {
                    // Reset the position of the previously hovered tile
                    if (previousHoveredTile != null)
                    {
                        StartCoroutine(ResetTilePosition(previousHoveredTile));
                    }
                    // Animate the new hovered tile
                    previousHoveredTile = hitObject;
                    StartCoroutine(AnimateTileHover(hitObject, hoverHeight));
                }

                // Handle left-click to select the tile
                if (Input.GetMouseButtonDown(0)) // Changed to left-click (0) from right-click (1)
                {
                    SelectedTile = hitObject;
                    Debug.Log($"Selected Tile: {SelectedTile.name}"); // Log the selected tile
                    StartCoroutine(ResetTilePosition(hitObject)); // Move the tile back to its default position
                }
            }
            else
            {
                // If we are no longer hovering over a tile, reset the previously hovered tile
                if (previousHoveredTile != null)
                {
                    StartCoroutine(ResetTilePosition(previousHoveredTile));
                    previousHoveredTile = null;
                }
            }
        }
        else
        {
            // If the raycast does not hit any tile, reset the previously hovered tile
            if (previousHoveredTile != null)
            {
                StartCoroutine(ResetTilePosition(previousHoveredTile));
                previousHoveredTile = null;
            }
        }
    }

    // Coroutine to animate the tile on hover
    private IEnumerator AnimateTileHover(GameObject tile, float targetHeight)
    {
        Vector3 startPosition = tile.transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, targetHeight, startPosition.z);
        float elapsedTime = 0f;

        while (elapsedTime < hoverSpeed)
        {
            tile.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / hoverSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tile.transform.position = targetPosition;
    }

    // Coroutine to reset the tile position to its default height (0)
    private IEnumerator ResetTilePosition(GameObject tile)
    {
        Vector3 startPosition = tile.transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, 1, startPosition.z); // Changed to reset to y=0 instead of 1
        float elapsedTime = 0f;

        while (elapsedTime < hoverSpeed)
        {
            tile.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / hoverSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tile.transform.position = targetPosition;
    }
}
