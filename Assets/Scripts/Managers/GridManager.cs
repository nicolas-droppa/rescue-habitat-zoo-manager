using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public Grid grid; // Referencia na Unity Grid komponent

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Vector3Int WorldToCell(Vector3 worldPos)
    {
        return grid.WorldToCell(worldPos);
    }

    public Vector3 CellToWorld(Vector3Int cellPos)
    {
        return grid.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f); // centrovanie do stredu tile-u
    }
}
