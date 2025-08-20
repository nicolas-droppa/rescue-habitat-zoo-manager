using UnityEngine;

public class WorkerStateWorking : WorkerState
{
    private float workDuration = 1.5f;
    private float workTimer;

    public WorkerStateWorking(Worker worker) : base(worker) {}

    public override void Enter()
    {
        workTimer = 0f;
    }

    public override void Update()
    {
        float deltaTime = Time.deltaTime * TimeManager.Instance.timeMultiplier;
        workTimer += deltaTime;

        if (workTimer >= workDuration)
        {
            worker.FinishJob();
        }
    }
}
