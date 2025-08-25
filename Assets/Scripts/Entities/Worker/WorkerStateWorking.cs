using UnityEngine;

public class WorkerStateWorking : WorkerState
{
    private float workDuration = 1.5f;
    private float workTimer;

    public WorkerStateWorking(Worker worker) : base(worker) {}

    public override void Enter()
    {
        workTimer = 0f;
        worker.StopWalking();
    }

    public override void Update()
    {
        workTimer += Time.deltaTime * TimeManager.Instance.timeMultiplier;

        if (workTimer >= workDuration)
        {
            worker.FinishJob();
        }
    }
}
