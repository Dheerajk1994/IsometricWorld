using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task
{
    public string TaskName { get; private set; }
    public bool IsDone { get; protected set; }
    public bool IsBeingValidated { get; protected set; }
    public Tuple<int, int> TaskLocation { get; protected set; }
    public GameObject Entity { get; protected set; }

    public static TerrainManager terrainMngInstance { get; protected set; }
    
    public event Action TaskCompleted = delegate { };
    public event Action<string> TaskFailed = delegate { };

    public Task(string taskName)
    {
        this.TaskName = taskName;
        if(terrainMngInstance == null)
        {
            terrainMngInstance = TerrainManager.instance;
        }
    }

    public abstract void AssignTaskToEntity(GameObject entity);
    public abstract bool IsValidated();
    public abstract void Validate(uint workAmount);
    public abstract void Execute(uint workAmount);
    protected virtual void OnFailure(string failureReason)
    {
        IsDone = false;
        TaskFailed(failureReason);
    }
    protected virtual void OnFinish()
    {
        IsDone = true;
        TaskCompleted();
    }
}
