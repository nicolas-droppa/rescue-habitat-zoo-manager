using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    private WorkerState currentState;

    public float moveSpeed = 2f;
    public Job currentJob;
    public Vector3 targetWorldPos;

    public List<Vector3> path;
    private int pathIndex = 0;

    [Header("References")]
    public Animator headAnimator;

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

        // Convert positions to grid cells
        Vector3Int startCell = GridManager.Instance.WorldToCell(transform.position);
        Vector3Int endCell = job.targetPosition;

        // If not walkable... find nearest walkable
        if (!GridManager.Instance.IsWalkable(endCell))
        {
            endCell = GridManager.Instance.FindNearestWalkableCell(endCell);
        }

        // Get path
        path = GridManager.Instance.FindPath(startCell, endCell);
        pathIndex = 0;

        if (path == null || path.Count == 0)
        {
            Debug.Log("No path to job!");
            ChangeState(new WorkerStateIdle(this));
            return;
        }

        // Set first waypoint
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
        {
            StopWalking();
            return;
        }

        float deltaTime = Time.deltaTime * TimeManager.Instance.timeMultiplier;

        Vector3 oldPos = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * deltaTime);

        // movement dir
        Vector3 dir = (transform.position - oldPos).normalized;

        if (dir.magnitude > 0.01f) // check for random dir...
        {
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            { 
                headAnimator.SetInteger("Direction", dir.x > 0 ? 3 : 2); // 3 = right, 2 = left
            }
            else
            {
                headAnimator.SetInteger("Direction", dir.y > 0 ? 1 : 0); // 1 = up, 0 = down
            }

            headAnimator.SetBool("IsWalking", true);
        }
        else
        {
            StopWalking();
        }

        // threshold so worker wont stop
        if (Vector3.Distance(transform.position, targetWorldPos) < 0.2f)
        {
            pathIndex++;
            if (pathIndex < path.Count)
            {
                targetWorldPos = path[pathIndex];
            }
            else
            {
                StopWalking();
                OnArrivedAtJob();
            }
        }
    }


    public void StopWalking()
    {
        headAnimator.SetBool("IsWalking", false);
    }
}
