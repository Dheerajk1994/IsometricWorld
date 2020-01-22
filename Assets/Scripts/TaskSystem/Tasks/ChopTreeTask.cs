using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopTreeTask : ComplexTask
{
    public ChopTreeTask(string taskName, Vector2Int taskLocation, float timeRequired)
        : base(taskName, taskLocation)
    {
        taskPrqQueue.Enqueue(new GoToTask("Going to " + TaskName, TaskLocation));
        taskPrqQueue.Enqueue(new TimedTask(timeRequired, TaskLocation, TaskName));
    }

    protected override void OnFinish()
    {
        base.OnFinish();
        Debug.Log("finished cutting tree");
        TerrainManager.instance.RemoveEntityFromWorld(TaskLocation);
        ResourceManager.instance.ResourceDropped(EntityType.Logs, TaskLocation, 10);
    }

    protected override void OnFailure(string failureReason)
    {
        base.OnFailure(failureReason);
    }

}
