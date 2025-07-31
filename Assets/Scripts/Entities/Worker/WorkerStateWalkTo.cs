using UnityEngine;

public class WorkerStateWalkTo : WorkerState
{
    public WorkerStateWalkTo(Worker worker) : base(worker) {}

    public override void Update()
    {
        worker.MoveTowardsTarget();

        if (worker.IsAtTarget())
        {
            worker.OnArrivedAtJob();
        }
    }
}
