using UnityEngine;

public class WorkerStateWorking : WorkerState
{
    private float workDuration = 1.5f; // čas potrebný na dokončenie úlohy
    private float workTimer;

    public WorkerStateWorking(Worker worker) : base(worker) {}

    public override void Enter()
    {
        workTimer = 0f;
    }

    public override void Update()
    {
        workTimer += Time.deltaTime;

        if (workTimer >= workDuration)
        {
            worker.FinishJob();
        }
    }
}

