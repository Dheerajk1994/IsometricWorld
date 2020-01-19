using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexTask : Task
{
    protected Queue<Task> taskPrqQueue;
    protected Task currentTask;

    public ComplexTask(string taskName, Vector2Int taskLocation) : base(taskName, taskLocation)
    {
        taskPrqQueue = new Queue<Task>();
    }

    public override void AssignTaskToEntity(GameObject entity)
    {
        this.Entity = entity;
    }

    public override void Execute(ref uint workAmount)
    {
        if (currentTask == null)
        {
            if (taskPrqQueue.Count > 0)
            {
                currentTask = taskPrqQueue.Peek();
                currentTask.TaskCompleted += PrereqCompleted;
                currentTask.TaskFailed += PrereqFailed;
                currentTask.AssignTaskToEntity(this.Entity);
            }
            else
            {
                OnFinish();
            }
        }
        else
        {
            currentTask.Execute(ref workAmount);
        }
    }

    protected virtual void PrereqCompleted()
    {
        taskPrqQueue.Dequeue();
        currentTask = null;
    }

    protected virtual void PrereqFailed(string reason)
    {
        taskPrqQueue.Dequeue();
        OnFailure("Complex task failed: " + reason);
    }
}
