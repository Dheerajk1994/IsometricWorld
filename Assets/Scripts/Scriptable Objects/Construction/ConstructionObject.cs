using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public struct ResourceAndAmount
{
    public EntityType resourceId;
    public uint resourceAmount;
}

abstract public class ConstructionObject : ScriptableObject
{
    public string buildingName;
    public Sprite buildingSprite;
    public EntityType constructionObjectID;
    public ResourceAndAmount[] constructionMaterials;
    public uint constructionTimeCost;
    public bool traversable;
    public float traversalRate;

    public abstract void FinishedBuilding();   

}
