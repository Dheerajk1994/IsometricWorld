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

    public event Action TaskCompleted = delegate { };
    public event Action<string> TaskFailed = delegate { };

    public Task(string taskName, Vector2Int taskLocation)
    {
        this.TaskName = taskName;
        this.TaskLocation = TaskLocation;
    }

    public abstract void AssignTaskToEntity(GameObject entity);
    public abstract void Execute(ref uint workAmount);
    protected virtual void OnFailure(string failureReason)
    {
        TaskFailed(failureReason);
    }
    protected virtual void OnFinish()
    {
        TaskCompleted();
    }
}
