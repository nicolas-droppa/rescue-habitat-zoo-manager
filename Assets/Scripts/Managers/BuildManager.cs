using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public enum BuildMode
{
    None,
    Build,
    Destroy
}

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    public Tilemap buildTilemap;
    public Tilemap previewTilemap;
    public RuleTile wallTile;
    private RuleTile selectedTile;

    private BuildMode currentMode = BuildMode.None;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void SelectBuildItem(BuildItem item)
    {
        if (item == null)
        {
            currentMode = BuildMode.None;
            selectedTile = null;
            ClearPreview();
            Debug.Log("Deselected build item");
        }
        else
        {
            currentMode = BuildMode.Build;
            selectedTile = item.tile;
            Debug.Log($"Selected: {item.itemName}");
        }
    }

    void Update()
    {
        HandleTileSelection();
        HandleTilePlacement(); // už NEVOLÁ UpdatePreview()
    }

    private void HandleTileSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentMode = BuildMode.Build;
            selectedTile = wallTile;
            Debug.Log("Mode: Build (Wall)");
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            currentMode = BuildMode.Destroy;
            selectedTile = null;
            Debug.Log("Mode: Destroy");
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            currentMode = BuildMode.None;
            selectedTile = null;
            ClearPreview();
            Debug.Log("Mode: None");
        }
    }

    // tu ukladáme všetky preview tiles, aby sme ich vedeli zmazať
    private List<Vector3Int> currentPreviewTiles = new List<Vector3Int>();

    private void ClearPreview()
    {
        foreach (var pos in currentPreviewTiles)
        {
            previewTilemap.SetTile(pos, null);
        }
        currentPreviewTiles.Clear();
    }

    private Vector3Int dragStartCellPos;
    private bool isDragging = false;

    private void HandleTilePlacement()
    {
        if (currentMode == BuildMode.None) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragStartCellPos = buildTilemap.WorldToCell(mouseWorldPos);
            isDragging = true;
            ClearPreview();
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int currentCellPos = buildTilemap.WorldToCell(mouseWorldPos);

            if (currentMode == BuildMode.Build && selectedTile != null)
            {
                ClearPreview();
                DrawRectanglePreview(dragStartCellPos, currentCellPos);
            }
            else if (currentMode == BuildMode.Destroy)
            {
                ClearPreview();
                DrawRectanglePreview(dragStartCellPos, currentCellPos, destroyMode: true);
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int endCellPos = buildTilemap.WorldToCell(mouseWorldPos);

            if (currentMode == BuildMode.Build && selectedTile != null)
            {
                ApplyRectangle(dragStartCellPos, endCellPos, selectedTile);
            }
            else if (currentMode == BuildMode.Destroy)
            {
                ApplyRectangle(dragStartCellPos, endCellPos, null);
            }

            ClearPreview();
            isDragging = false;
        }
    }

    private void DrawRectanglePreview(Vector3Int start, Vector3Int end, bool destroyMode = false)
    {
        int minX = Mathf.Min(start.x, end.x);
        int maxX = Mathf.Max(start.x, end.x);
        int minY = Mathf.Min(start.y, end.y);
        int maxY = Mathf.Max(start.y, end.y);

        for (int x = minX; x <= maxX; x++)
        {
            Vector3Int pos1 = new Vector3Int(x, minY, 0);
            Vector3Int pos2 = new Vector3Int(x, maxY, 0);

            previewTilemap.SetTile(pos1, destroyMode ? null : selectedTile);
            previewTilemap.SetTile(pos2, destroyMode ? null : selectedTile);

            currentPreviewTiles.Add(pos1);
            currentPreviewTiles.Add(pos2);
        }

        for (int y = minY; y <= maxY; y++)
        {
            Vector3Int pos1 = new Vector3Int(minX, y, 0);
            Vector3Int pos2 = new Vector3Int(maxX, y, 0);

            previewTilemap.SetTile(pos1, destroyMode ? null : selectedTile);
            previewTilemap.SetTile(pos2, destroyMode ? null : selectedTile);

            currentPreviewTiles.Add(pos1);
            currentPreviewTiles.Add(pos2);
        }
    }

    private void ApplyRectangle(Vector3Int start, Vector3Int end, TileBase tile)
    {
        int minX = Mathf.Min(start.x, end.x);
        int maxX = Mathf.Max(start.x, end.x);
        int minY = Mathf.Min(start.y, end.y);
        int maxY = Mathf.Max(start.y, end.y);

        for (int x = minX; x <= maxX; x++)
        {
            buildTilemap.SetTile(new Vector3Int(x, minY, 0), tile);
            buildTilemap.SetTile(new Vector3Int(x, maxY, 0), tile);
        }

        for (int y = minY; y <= maxY; y++)
        {
            buildTilemap.SetTile(new Vector3Int(minX, y, 0), tile);
            buildTilemap.SetTile(new Vector3Int(maxX, y, 0), tile);
        }
    }
}
