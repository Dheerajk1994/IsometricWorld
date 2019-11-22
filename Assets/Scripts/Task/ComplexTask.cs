using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexTask : Task
{
    protected Queue<Task> taskPrqQueue;
    protected Task currentTask;

    public ComplexTask(string taskName, Tuple<int, int> taskLocation) : base(taskName)
    {
        this.TaskLocation = taskLocation;
        taskPrqQueue = new Queue<Task>();
    }

    public override void AssignTaskToEntity(GameObject entity)
    {
        this.Entity = entity;
    }

    public override void Execute(uint workAmount)
    {
        if (currentTask != null)
        {
            //NEEDS REVISION    
            if (currentTask.IsValidated())
            {
                currentTask.Execute(workAmount);
            }
            else if(!currentTask.IsBeingValidated)
            {
                currentTask.Validate(workAmount);
            }
        }
        else
        {
            Validate(workAmount);
        }
    }

    public override bool IsValidated()
    {
        return true;
    }
    
    public override void Validate(uint workAmount)
    {
        if (taskPrqQueue.Count > 0)
        {
            currentTask = taskPrqQueue.Peek();
            currentTask.TaskCompleted += PrereqCompleted;
            currentTask.AssignTaskToEntity(this.Entity);
        }
        else
        {
            OnFinish();
        }
    }

    protected virtual void PrereqCompleted()
    {
        taskPrqQueue.Dequeue();
        currentTask = null;
    }
}
