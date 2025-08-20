using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public Grid grid;

    // Track which tiles are blocked (walls, etc.)
    private HashSet<Vector3Int> blockedTiles = new HashSet<Vector3Int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Conversion helpers
    public Vector3Int WorldToCell(Vector3 worldPos)
    {
        return grid.WorldToCell(worldPos);
    }

    public Vector3 CellToWorld(Vector3Int cellPos)
    {
        return grid.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f); // center of tile
    }

    // Walkability tracking
    public bool IsWalkable(Vector3Int cellPos)
    {
        return !blockedTiles.Contains(cellPos);
    }

    public void SetBlocked(Vector3Int cellPos, bool blocked)
    {
        if (blocked) blockedTiles.Add(cellPos);
        else blockedTiles.Remove(cellPos);
    }

    // Pathfinding (BFS) - returns world positions
    public List<Vector3> FindPath(Vector3Int start, Vector3Int end)
    {
        Queue<Vector3Int> frontier = new Queue<Vector3Int>();
        frontier.Enqueue(start);

        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        cameFrom[start] = start;

        Vector3Int[] directions = {
            Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
        };

        while (frontier.Count > 0)
        {
            Vector3Int current = frontier.Dequeue();

            if (current == end) break;

            foreach (var dir in directions)
            {
                Vector3Int next = current + dir;
                if (!cameFrom.ContainsKey(next) && IsWalkable(next))
                {
                    frontier.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
        }

        if (!cameFrom.ContainsKey(end)) return null; // no path

        // Reconstruct path in world positions
        List<Vector3> worldPath = new List<Vector3>();
        Vector3Int curr = end;
        while (curr != start)
        {
            worldPath.Add(CellToWorld(curr));
            curr = cameFrom[curr];
        }
        worldPath.Reverse();
        return worldPath;
    }
}
