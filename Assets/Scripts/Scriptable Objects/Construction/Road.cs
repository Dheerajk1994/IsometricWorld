using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Road", menuName = "ScriptableObjects/Building/Road")]
public class Road : ConstructionObject
{
    public override void FinishedBuilding()
    {
        //TerrainManager.instance.AddBuildingToWorld()
    }
}
