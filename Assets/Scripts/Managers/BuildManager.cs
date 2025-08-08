using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    public Tilemap buildTilemap;          // Tilemap pre stavbu
    public Tilemap previewTilemap;        // Tilemap pre preview (v inej vrstve)
    public RuleTile wallTile;
    private RuleTile selectedTile;
    private Vector3Int lastPreviewPos = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    void Update()
    {
        HandleTileSelection();
        UpdatePreview();
        HandleTilePlacement();
    }

    private void HandleTileSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            selectedTile = wallTile;
            Debug.Log("Selected tile: Wall");
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            selectedTile = null;
            ClearPreview();
            Debug.Log("Deselected tile");
        }
    }

    private void UpdatePreview()
    {
        if (selectedTile == null)
        {
            ClearPreview();
            return;
        }

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = buildTilemap.WorldToCell(mouseWorldPos);

        if (cellPos != lastPreviewPos)
        {
            ClearPreview();
            previewTilemap.SetTile(cellPos, selectedTile);
            lastPreviewPos = cellPos;
        }
    }

    private void ClearPreview()
    {
        if (lastPreviewPos != new Vector3Int(int.MinValue, int.MinValue, int.MinValue))
        {
            previewTilemap.SetTile(lastPreviewPos, null);
            lastPreviewPos = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
        }
    }

    private Vector3Int dragStartCellPos;
    private bool isDragging = false;
    private bool dragDirectionLocked = false;
    private bool lockVertical = false; // true = vertical, false = horizontal
    private Vector3Int lastPlacedTilePos = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);

    private void HandleTilePlacement()
    {
        if (selectedTile == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragStartCellPos = buildTilemap.WorldToCell(mouseWorldPos);
            lastPlacedTilePos = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
            isDragging = true;
            dragDirectionLocked = false;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int currentCellPos = buildTilemap.WorldToCell(mouseWorldPos);

            if (!dragDirectionLocked && currentCellPos != dragStartCellPos)
            {
                int dx = Mathf.Abs(currentCellPos.x - dragStartCellPos.x);
                int dy = Mathf.Abs(currentCellPos.y - dragStartCellPos.y);

                lockVertical = dy >= dx;
                dragDirectionLocked = true;
            }

        
            Vector3Int lockedCellPos = currentCellPos;

            if (dragDirectionLocked)
            {
                if (lockVertical)
                    lockedCellPos.x = dragStartCellPos.x; // iba Y sa mení
                else
                    lockedCellPos.y = dragStartCellPos.y; // iba X sa mení
            }

            if (lockedCellPos != lastPlacedTilePos)
            {
                buildTilemap.SetTile(lockedCellPos, selectedTile);
                lastPlacedTilePos = lockedCellPos;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            dragDirectionLocked = false;
            lastPlacedTilePos = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
        }
    }
}
