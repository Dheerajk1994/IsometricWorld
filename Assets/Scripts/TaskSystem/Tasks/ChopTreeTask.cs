using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopTreeTask : ComplexTask
{
    public ChopTreeTask(string taskName, Vector2Int taskLocation, float timeRequired)
        : base(taskName, taskLocation)
    {
        taskPrqQueue.Enqueue(new TimedTask(timeRequired, taskLocation, taskName));
    }

    protected override void OnFinish()
    {
        base.OnFinish();
        Debug.Log("finished cutting tree");
        TerrainManager.instance.RemoveEntityFromWorld(TaskLocation);
        TerrainManager.instance.AddEntityToWorld(TaskLocation, StaticEntityType.Logs);
    }

    protected override void OnFailure(string failureReason)
    {
        base.OnFailure(failureReason);
    }

}
