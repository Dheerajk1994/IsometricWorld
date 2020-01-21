using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;
    [SerializeField]private List<ResourceStorage> resourceStorageList;
    private Dictionary<StaticEntityType, uint> resourceAndAmountDict;

    private List<ResourceEntity> entitiesLyingInWorld;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(this != instance)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        if(resourceStorageList == null)
        {
            resourceStorageList = new List<ResourceStorage>();
        }
        resourceAndAmountDict = new Dictionary<StaticEntityType, uint>();
        entitiesLyingInWorld = new List<ResourceEntity>();
    }

    private void Update()
    {
        //see if there are any resources lying around and add them for hauling
        if(entitiesLyingInWorld.Count > 0)
        {
            ResourceStorage storage = GetClosestResourceStorageWithItem(entitiesLyingInWorld[0].CellIndex, entitiesLyingInWorld[0].EntityType);
            if(storage != null)
            {
                //Debug.Log("closest storage area at " + storage.positionCellIndex);
                ResourceEntity resourceEntity = entitiesLyingInWorld[0];
                TaskManager.instance.AddTask( new HaulTask("Haul " + resourceEntity.EntityType, resourceEntity, storage, resourceEntity.EntityType));
                entitiesLyingInWorld.RemoveAt(0);
            }
        }
    }   

    public void AddResourceStorage(ResourceStorage resourceStorage)
    {
        //Debug.Log("added new resource storage area");
        resourceStorageList.Add(resourceStorage);
        if(resourceStorage.storedResourceType != StaticEntityType.Empty)
        {
            if (!resourceAndAmountDict.ContainsKey(resourceStorage.storedResourceType))
            {
                resourceAndAmountDict.Add(resourceStorage.storedResourceType, resourceStorage.currentAmountInInventory);
            }
        }
        //adding delegates
        resourceStorage.ResourceAddedHandler += ResourceWasAdded;
        resourceStorage.ResourceRemovedHandler += ResourceWasRemoved;
    }

    //FETCH LIST OF STORAGE AREAS WITH GIVEN RESOURCE TYPE
    public List<ResourceStorage> GetStorageAreas(StaticEntityType resourceEnum)
    {
        List<ResourceStorage> storageList = new List<ResourceStorage>();
        foreach(ResourceStorage storage in resourceStorageList)
        {
            if(storage.storedResourceType == resourceEnum && storage.currentAmountInInventory > 0)
            {
                storageList.Add(storage);
            }
        }
        return storageList;
    }

    //FETCH THE CLOSEST STORAGE AREA WITH GIVEN TYPE
    public ResourceStorage GetClosestResourceStorageWithItem(in Vector2 pos, StaticEntityType resourceEnum)
    {
        float dist = -1;
        ResourceStorage closestStorage = null;
        foreach (ResourceStorage resourceStorage in resourceStorageList)
        {
            if(resourceStorage.storedResourceType == resourceEnum)
            {
                float potDistnace = TerrainManager.instance.GetDistanceBetween(pos, resourceStorage.positionCellIndex);
                if (dist == -1)
                {
                    dist = potDistnace;
                    closestStorage = resourceStorage;
                }
                else
                {
                    if(potDistnace < dist)
                    {
                        dist = potDistnace;
                        closestStorage = resourceStorage;
                    }
                }
            }
        }
        return closestStorage;
    }

    //FETCH THE CLOSEST STORAGE AREA TO PLACE ITEM
    public ResourceStorage GetClosestResourceStorageToStoreItem(in Vector2 pos, StaticEntityType resourceEnum)
    {
        float dist = -1;
        ResourceStorage closestStorage = null;
        foreach (ResourceStorage resourceStorage in resourceStorageList)
        {
            if (resourceStorage.storedResourceType == resourceEnum || resourceStorage.storedResourceType == StaticEntityType.Empty)
            {
                float potDistnace = TerrainManager.instance.GetDistanceBetween(pos, resourceStorage.positionCellIndex);
                if (dist == -1)
                {
                    dist = potDistnace;
                    closestStorage = resourceStorage;
                }
                else
                {
                    if (potDistnace < dist)
                    {
                        dist = potDistnace;
                        closestStorage = resourceStorage;
                    }
                }
            }
        }
        return closestStorage;
    }

    //DETECT RESOURCE ADD
    private void ResourceWasAdded(StaticEntityType resourceEnum, uint amnt)
    {
        resourceAndAmountDict[resourceEnum] += amnt;
    }

    //DETECT RESOURCE REMOVE
    private void ResourceWasRemoved(StaticEntityType resourceEnum, uint amnt)
    {
        //CAREFUL
        resourceAndAmountDict[resourceEnum] -= amnt;
    }

    public void ResourceDropped(StaticEntityType resourceEnum, Vector2Int tileIndex, int amount)
    {
        Debug.Log("Resource " + resourceEnum.ToString() + " dropped");
        TerrainManager.instance.AddEntityToWorld(tileIndex, resourceEnum);
        ResourceEntity rEntity = new ResourceEntity(resourceEnum.ToString(), resourceEnum, tileIndex, null);
        entitiesLyingInWorld.Add(rEntity);
    }
}
