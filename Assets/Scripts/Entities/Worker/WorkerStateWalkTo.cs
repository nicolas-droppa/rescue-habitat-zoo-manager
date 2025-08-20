public class WorkerStateWalkTo : WorkerState
{
    public WorkerStateWalkTo(Worker worker) : base(worker) { }

    public override void Enter()
    {
        // Nothing special for now
    }

    public override void Update()
    {
        // Just move along the path each frame
        worker.MoveAlongPath();
    }

    public override void Exit()
    {
        // Nothing to clean up
    }
}
