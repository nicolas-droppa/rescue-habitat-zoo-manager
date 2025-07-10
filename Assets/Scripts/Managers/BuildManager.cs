using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    public Tilemap buildTilemap;
    public TileBase wallTile;

    private TileBase selectedTile;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    void Update()
    {
        HandleTileSelection();
        HandleTilePlacement();
    }

    private void HandleTileSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            selectedTile = wallTile;
            Debug.Log("Selected tile: Wall");
        }
    }

    private void HandleTilePlacement()
    {
        if (Input.GetMouseButtonDown(0) && selectedTile != null) {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = buildTilemap.WorldToCell(mouseWorldPos);
            buildTilemap.SetTile(cellPos, selectedTile);
        }
    }
}