using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTask : ComplexTask
{
    protected ConstructionObject constructionObject { get; private set; }
    protected List<ResourceAndAmount> materialsNeeded;
    protected VoidInventory constructionMaterialsBox;

    public BuildTask(ConstructionObject constructionObject, string taskName, Vector2Int mainTaskLocation)
        :
        base(taskName, mainTaskLocation)
    {
        this.constructionObject = constructionObject;
        this.materialsNeeded = new List<ResourceAndAmount>();
        constructionMaterialsBox = new VoidInventory(mainTaskLocation);
        
    }

    public override void Execute(ref uint workAmount)
    {
        if (!isValid)
        {
            foreach (ResourceAndAmount ra in constructionObject.constructionMaterials)
            {
                HaulTask haul = new HaulTask("Haul " + ra.resourceId.ToString(), constructionMaterialsBox, ra.resourceId, 10);
                haul.TaskFailedHandler += OnFailure;
                taskPrqQueue.Enqueue(haul);
            }
            taskPrqQueue.Enqueue(new GoToTask(" ", this.TaskLocation));
            taskPrqQueue.Enqueue(new TimedTask(constructionObject.constructionTimeCost, this.TaskLocation, "Build " + constructionObject.buildingName));
            isValid = true;
        }
        else
        {
            base.Execute(ref workAmount);
        }
    }


    protected override void OnFinish()
    {
        TerrainManager.instance.AddBuildingToWorld(constructionObject, this.TaskLocation);
        base.OnFinish();
    }

    protected override void OnFailure(string failureReason)
    {
        base.OnFailure(failureReason);
    }

}

