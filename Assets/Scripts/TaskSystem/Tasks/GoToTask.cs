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
            this.Entity.GetComponent<EntityMovement>().Move(TerrainManager.instance.RequestPath(Entity.transform.position, TaskLocation));
            this.Entity.GetComponent<EntityMovement>().DestinationReachedHandler += OnFinish;
            isValid = true;
        }
    }

    protected override void OnFinish()
    {
        this.Entity.GetComponent<EntityMovement>().DestinationReachedHandler -= OnFinish;
        base.OnFinish();
    }


}
