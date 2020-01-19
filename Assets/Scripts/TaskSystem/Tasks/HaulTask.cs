using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulTask : ComplexTask
{
    StaticEntityType itemHauling;
    Vector2Int from;
    IGrabFrom grabFrom;
    IDropOff dropOff;
    StaticEntityType resourceType;
    int itemAmount;
    public HaulTask(string taskName, Vector2Int from, Vector2Int to, IGrabFrom grabFrom, IDropOff dropOff, StaticEntityType resourceType)
        : 
        base(taskName, to)
    {
        Debug.Log("haul task from " + from + " to " + to);
        this.from = from;
        this.dropOff = dropOff;
        this.grabFrom = grabFrom;
        this.resourceType = resourceType;

        GoToTask grabTask = new GoToTask(taskName, from);
        grabTask.TaskCompleted += ReachedSourceLocation;
        taskPrqQueue.Enqueue(grabTask);


        GoToTask dropTask = new GoToTask(taskName, to);
        dropTask.TaskCompleted += ReachedDestinationLocation;
        taskPrqQueue.Enqueue(dropTask);
    }

    public void ReachedSourceLocation() {
        this.Entity.GetComponent<EntityInventory>().AddInventoryItem(resourceType, grabFrom.Grab(10));
    }

    public void ReachedDestinationLocation() {
        dropOff.DropOff(resourceType, this.Entity.GetComponent<EntityInventory>().RemoveInventoryItem(resourceType, 10));
    }

}
