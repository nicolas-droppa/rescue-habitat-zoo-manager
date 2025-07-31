using UnityEngine;

public class Worker : MonoBehaviour
{
    private WorkerState currentState;

    public float moveSpeed = 2f;
    public Job currentJob;
    public Vector3 targetWorldPos;

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
        targetWorldPos = TileToWorld(job.targetPosition);
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

    public void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);
    }

    public bool IsAtTarget()
    {
        return Vector3.Distance(transform.position, targetWorldPos) < 0.1f;
    }

    public Vector3 TileToWorld(Vector3Int tilePos)
    {
        if (GridManager.Instance == null)
        {
            Debug.LogError("GridManager.Instance is null!");
            return transform.position; // fallback to current position
        }

        if (GridManager.Instance.grid == null)
        {
            Debug.LogError("GridManager.Instance.grid is null!");
            return transform.position;
        }

        return GridManager.Instance.grid.CellToWorld(tilePos) + new Vector3(0.5f, 0.5f);
    }
}
