using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedTask : Task
{
    protected float timeValue = 0;
    private float currentTimeValue = 0;

    public TimedTask(float timeValue, Vector2Int taskLocation, string taskName) : base(taskName, taskLocation)
    {
        this.timeValue = timeValue;
    }

    public override void AssignTaskToEntity(GameObject entity)
    {
        this.Entity = entity;
    }

    public override void Execute(ref uint workAmount)
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
}
