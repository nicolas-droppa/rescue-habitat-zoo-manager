using UnityEngine;

public class WorkerStateWalkTo : WorkerState
{
    public WorkerStateWalkTo(Worker worker) : base(worker) { }

    public override void Enter()
    {
        // pri vstupe do walk sa zapne walking anim
        worker.headAnimator.SetBool("IsWalking", true);
    }

    public override void Update()
    {
        worker.MoveAlongPath();
    }

    public override void Exit()
    {
        // pri výstupe vypneme walking
        worker.headAnimator.SetBool("IsWalking", false);
    }
}
