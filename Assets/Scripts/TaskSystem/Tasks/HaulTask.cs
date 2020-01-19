using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulTask : ComplexTask
{
    StaticEntityType itemHauling;
    Vector2Int sourceLocation;
    IGrabFrom grabFrom;
    int itemAmount;
    public HaulTask(string taskName, Vector2Int taskLocation, IGrabFrom grabFrom) : base(taskName, taskLocation)
    {
        this.grabFrom = grabFrom;
        taskPrqQueue.Enqueue(new GoToTask(taskName, sourceLocation));
        taskPrqQueue.Enqueue(new GoToTask(taskName, taskLocation));
    }

    public void ReachedSourceLocation() { }

    public void ReachedDestinationLocation() { }

}
