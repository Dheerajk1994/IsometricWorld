using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task
{
    public string TaskName { get; private set; }
    public bool IsBeingValidated { get; protected set; }
    public Vector2Int TaskLocation { get; protected set; }
    public GameObject Entity { get; protected set; }
    protected bool isValid = false;

    public event Action TaskCompletedHandler = delegate { };
    public event Action<string> TaskFailedHandler = delegate { };

    public Task(string TaskName, Vector2Int TaskLocation)
    {
        this.TaskName = TaskName;
        this.TaskLocation = TaskLocation;
    }

    public abstract void AssignTaskToEntity(GameObject entity);
    public abstract void Execute(ref uint workAmount);
   
    protected virtual void OnFinish()
    {
        TaskCompletedHandler();
    }
    protected virtual void OnFailure(string failureReason)
    {
        Debug.Log(TaskName + " failed : " + failureReason);
        TaskFailedHandler(failureReason);
    }
}
