using UnityEngine;

public class Worker : MonoBehaviour
{
    private Job currentJob;
    private Vector3 targetWorldPos;
    public float moveSpeed = 2f;

    void Update()
    {
        if (currentJob == null)
        {
            currentJob = JobSystem.Instance.GetNextJob();
            if (currentJob != null)
                targetWorldPos = TileToWorld(currentJob.targetPosition);
        }

        if (currentJob != null)
        {
            MoveTowardsTarget();

            if (Vector3.Distance(transform.position, targetWorldPos) < 0.1f)
            {
                currentJob.onComplete?.Invoke();
                currentJob = null; // finish job
            }
        }
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);
    }

    Vector3 TileToWorld(Vector3Int tilePos)
    {
        return GridManager.Instance.grid.CellToWorld(tilePos) + new Vector3(0.5f, 0.5f); // center of tile
    }
}