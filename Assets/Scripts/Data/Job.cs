using System.Collections.Generic;
using UnityEngine;

public class Job
{
    public Vector3Int targetPosition;
    public System.Action onComplete;

    public Job(Vector3Int targetPosition, System.Action onComplete)
    {
        this.targetPosition = targetPosition;
        this.onComplete = onComplete;
    }
}