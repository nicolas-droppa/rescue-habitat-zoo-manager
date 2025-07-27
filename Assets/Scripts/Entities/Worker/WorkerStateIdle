public class WorkerStateIdle : WorkerState
{
    public WorkerStateIdle(Worker worker) : base(worker) {}

    public override void Enter()
    {
        // Idle anim
    }

    public override void Update()
    {
        var job = JobSystem.Instance.GetNextJob();
        if (job != null)
        {
            worker.SetJob(job);
        }
    }
}
