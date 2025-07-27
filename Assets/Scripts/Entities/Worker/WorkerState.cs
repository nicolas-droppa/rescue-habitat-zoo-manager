public abstract class WorkerState
{
    protected Worker worker;

    public WorkerState(Worker worker)
    {
        this.worker = worker;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
