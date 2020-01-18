using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTask : ComplexTask
{
    public ConstructionObject constructionObject { get; private set; }
    public List<ResourceAndAmount> materialsNeeded;
    
    public BuildTask(ConstructionObject constructionObject, string taskName, Vector2Int mainTaskLocation) : base(taskName, mainTaskLocation)
    {
        this.constructionObject = constructionObject;
        materialsNeeded = new List<ResourceAndAmount>();
        for(int i = 0; i < constructionObject.constructionMaterials.Length; ++i)
        {
            materialsNeeded.Add(constructionObject.constructionMaterials[i]);
        }

        //POPULATE PREREQ TASKS BASED ON MATERIALS NEEDED AT SITE
        foreach (ResourceAndAmount materials in materialsNeeded)
        {
            taskPrqQueue.Enqueue(new FetchMaterialTask("FETCH.. ", materials, mainTaskLocation));
        }

        //ADD FINAL TASK - THE BUILDING ITSELF AFTER ALL PREREQ
        taskPrqQueue.Enqueue(new TimedTask(constructionObject.constructionTimeCost, mainTaskLocation,"Build " + constructionObject.buildingName));


        //Debug.Log("build task created");
    }

    protected override void OnFinish()
    {
        base.OnFinish();
        TerrainManager.instance.AddBuildingToWorld(constructionObject, this.TaskLocation);
    }

    protected override void OnFailure(string failureReason)
    {
        base.OnFailure(failureReason);
    }

}
