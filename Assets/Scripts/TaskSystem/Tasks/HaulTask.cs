using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulTask : ComplexTask
{
    IGrabFrom grabFrom;
    IDropOff dropOff;
    EntityType resourceType;
    int itemAmount;

    //WHEN FROM AND TO ARE KNOWN
    public HaulTask(string taskName, IGrabFrom grabFrom, IDropOff dropOff, EntityType resourceType)
        : 
        base(taskName, dropOff.GetLocation())
    {
        this.dropOff = dropOff;
        this.grabFrom = grabFrom;
        this.resourceType = resourceType;
    }

    //WHEN FROM IS NOT KNOWN - USED FOR LOOKING FOR ITEMS IN STOCKPILE
    public HaulTask(string taskName, IDropOff dropOff, EntityType resourceType, int amount)
        :
        base(taskName, dropOff.GetLocation())
    {
        //this.TaskFailed += onFailure;
        this.dropOff = dropOff;
        this.resourceType = resourceType;
        this.itemAmount = amount;
        grabFrom = ResourceManager.instance.GetClosestResourceStorageWithItem(TerrainManager.instance.GetWorldPosGivenTileIndex(dropOff.GetLocation()), resourceType);
        
    }

    public override void Execute(ref uint workAmount)
    {
        if (!isValid)
        {
            if (grabFrom != null)
            {
                GoToTask grabTask = new GoToTask(this.TaskName, grabFrom.GetLocation());
                grabTask.TaskCompletedHandler += ReachedSourceLocation;
                taskPrqQueue.Enqueue(grabTask);

                GoToTask dropTask = new GoToTask(this.TaskName, dropOff.GetLocation());
                dropTask.TaskCompletedHandler += ReachedDestinationLocation;
                taskPrqQueue.Enqueue(dropTask);

                isValid = true;
            }
            else
            {
                OnFailure("Couldnt find resource: " + resourceType.ToString());
            }
        }
        else
        {
            base.Execute(ref workAmount);
        }
    }

    public override void AssignTaskToEntity(GameObject entity)
    {
        this.Entity = entity;
    }

    public void ReachedSourceLocation() {
        this.Entity.GetComponent<EntityInventory>().AddInventoryItem(resourceType, grabFrom.Grab(10));
    }

    public void ReachedDestinationLocation() {
        dropOff.DropOff(resourceType, (uint)this.Entity.GetComponent<EntityInventory>().RemoveInventoryItem(resourceType, 10));
    }

}
