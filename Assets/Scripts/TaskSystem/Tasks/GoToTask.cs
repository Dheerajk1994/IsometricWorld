using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTask : Task
{
    public GoToTask(string taskName, Vector2Int taskLocation)
        :
        base(taskName, taskLocation)
    { }

    public override void AssignTaskToEntity(GameObject entity)
    {
        this.Entity = entity;
    }

    public override void Execute(ref uint workAmount)
    {
        if (!isValid)
        {
            //Debug.Log("requesting path to " + TaskLocation);
            this.Entity.GetComponent<EntityMovement>().Move(TerrainManager.instance.RequestPath(Entity.transform.position, TaskLocation));
            this.Entity.GetComponent<EntityMovement>().DestinationReachedHandler += OnFinish;
            this.Entity.GetComponent<EntityMovement>().DestinationNotReachableHandler += OnFailure;
            isValid = true;
        }
    }

    protected override void OnFinish()
    {
        this.Entity.GetComponent<EntityMovement>().DestinationReachedHandler -= OnFinish;
        base.OnFinish();
    }

    protected override void OnFailure(string failureReason)
    {
        this.Entity.GetComponent<EntityMovement>().DestinationNotReachableHandler -= OnFailure;
        base.OnFailure(failureReason);
    }

}
