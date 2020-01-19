using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchMaterialTask : Task
{
    public ResourceAndAmount MaterialToFetch { get; private set; }
    public int CurrentAmount { get; private set; }

    private bool isValidated = false;

    public FetchMaterialTask(string taskName, ResourceAndAmount materialToFetch, Vector2Int FetchLocation) : base (taskName, FetchLocation)
    {
        this.MaterialToFetch = materialToFetch;
    }

    public override void AssignTaskToEntity(GameObject entity)
    {
        this.Entity = entity;
    }

    public override void Execute(ref uint workAmount)
    {
        Debug.Log("fetch materials");
        if (!isValidated)
        {
            Validate(ref workAmount);
        }
    }

    protected void Validate(ref uint workAmount)
    {
        ResourceStorage closestStorage = ResourceManager.instance.GetClosestResourceStorageWithItem(this.Entity.transform.position, this.MaterialToFetch.resourceId);
        if(closestStorage != null)
        {
            Vector2Int closestStorageIndex = closestStorage.positionCellIndex; 
            Entity.GetComponent<EntityMovement>().Move(TerrainManager.instance.RequestPath(Entity.transform.position, closestStorageIndex));
            Entity.GetComponent<EntityMovement>().DestinationReachedHandler += ReachedStorageLocation;
            isValidated = true;
        }
        else
        {
            isValidated = false;
            OnFailure("Cannot find storage area.");
        }
    }

    private void ReachedStorageLocation()
    {
        this.Entity.GetComponent<EntityMovement>().DestinationReachedHandler -= ReachedStorageLocation;
        this.Entity.GetComponent<EntityMovement>().Move(TerrainManager.instance.RequestPath((Vector2)this.Entity.transform.position, this.TaskLocation));
        this.Entity.GetComponent<EntityMovement>().DestinationReachedHandler += ReachedConstructionLocation;
    }

    private void ReachedConstructionLocation()
    {
        Entity.GetComponent<EntityMovement>().DestinationReachedHandler -= ReachedConstructionLocation;
        OnFinish();
    }

}
