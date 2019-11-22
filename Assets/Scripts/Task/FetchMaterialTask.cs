using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchMaterialTask : Task
{
    public ResourceAndAmount MaterialToFetch { get; private set; }
    public int CurrentAmount { get; private set; }

    private bool isValidated = false;

    public FetchMaterialTask(string taskName, ResourceAndAmount materialToFetch, Tuple<int, int> FetchLocation) : base (taskName)
    {
        this.MaterialToFetch = materialToFetch;
        this.TaskLocation = FetchLocation;
    }

    public override void AssignTaskToEntity(GameObject entity)
    {
        this.Entity = entity;
    }

    public override void Execute(uint workAmount)
    {
        if (!isValidated)
        {
            Validate(workAmount);
        }
    }

    public override bool IsValidated()
    {
        return isValidated;
    }

    public override void Validate(uint workAmount)
    {
        Vector2 storagePos = ResourceManager.instance.getStorageAreaLocation(MaterialToFetch.resourceId);
        Entity.GetComponent<EntityMovement>().Move(TerrainManager.instance.RequestPath(Entity.transform.position, storagePos));
        Entity.GetComponent<EntityMovement>().DestinationReached += ReachedStorageLocation;
        isValidated = true;
    }

    private void ReachedStorageLocation()
    {
        this.Entity.GetComponent<EntityMovement>().DestinationReached -= ReachedStorageLocation;
        this.Entity.GetComponent<EntityMovement>().Move(TerrainManager.instance.RequestPath((Vector2)this.Entity.transform.position, this.TaskLocation));
        this.Entity.GetComponent<EntityMovement>().DestinationReached += ReachedConstructionLocation;
    }

    private void ReachedConstructionLocation()
    {
        Entity.GetComponent<EntityMovement>().DestinationReached -= ReachedConstructionLocation;
        IsDone = true;
        OnFinish();
    }

}
