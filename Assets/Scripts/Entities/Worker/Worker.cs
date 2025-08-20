using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    private WorkerState currentState;

    public float moveSpeed = 2f;
    public Job currentJob;
    public Vector3 targetWorldPos;

    // Path from GridManager
    public List<Vector3> path;
    private int pathIndex = 0;

    void Start()
    {
        ChangeState(new WorkerStateIdle(this));
    }

    void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(WorkerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void SetJob(Job job)
    {
        currentJob = job;

        // Find path using GridManager
        Vector3Int startCell = GridManager.Instance.WorldToCell(transform.position);
        Vector3Int endCell = job.targetPosition;

        path = GridManager.Instance.FindPath(startCell, endCell);
        pathIndex = 0;

        if (path == null || path.Count == 0)
        {
            Debug.Log("No path to job!");
            ChangeState(new WorkerStateIdle(this));
            return;
        }

        targetWorldPos = path[pathIndex];
        ChangeState(new WorkerStateWalkTo(this));
    }

    public void OnArrivedAtJob()
    {
        ChangeState(new WorkerStateWorking(this));
    }

    public void FinishJob()
    {
        currentJob?.onComplete?.Invoke();
        currentJob = null;
        ChangeState(new WorkerStateIdle(this));
    }

    public void MoveAlongPath()
    {
        if (path == null || pathIndex >= path.Count)
            return;

        transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWorldPos) < 0.1f)
        {
            pathIndex++;
            if (pathIndex < path.Count)
            {
                targetWorldPos = path[pathIndex];
            }
            else
            {
                // Reached final destination
                OnArrivedAtJob();
            }
        }
    }
}
