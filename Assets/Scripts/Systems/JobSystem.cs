using System.Collections.Generic;
using UnityEngine;

public class JobSystem : MonoBehaviour
{
    public static JobSystem Instance;

    private Queue<Job> jobQueue = new Queue<Job>();

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void EnqueueJob(Job job)
    {
        jobQueue.Enqueue(job);
    }

    public Job GetNextJob()
    {
        if (jobQueue.Count > 0)
            return jobQueue.Dequeue();
        else
            return null;
    }
}