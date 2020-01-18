using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedTask : Task
{
    protected float timeValue = 0;
    private float currentTimeValue = 0;

    public TimedTask(float timeValue, Vector2Int taskLocation, string taskName) : base(taskName)
    {
        this.timeValue = timeValue;
        this.TaskLocation = taskLocation;
    }

    public override void AssignTaskToEntity(GameObject entity)
    {
        this.Entity = entity;
    }

    public override void Execute(uint workAmount)
    {
        if(currentTimeValue < timeValue)
        {
            currentTimeValue += workAmount;
        }
        else
        {
            OnFinish();
        }
    }

    public override bool IsValidated()
    {
        return (terrainMngInstance.GetTilePosGivenWorldPos(this.Entity.transform.position).Equals(TaskLocation));
    }

    public override void Validate(uint workAmount)
    {
        this.Entity.GetComponent<EntityMovement>().Move(terrainMngInstance.RequestPath(this.Entity.transform.position, TaskLocation));
        IsBeingValidated = true;
    }
}
