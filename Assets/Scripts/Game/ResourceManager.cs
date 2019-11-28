using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;
    [SerializeField]private List<ResourceStorage> resourceStorageList;
    private Dictionary<ResourceEnum, uint> resourceAndAmountDict;

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
        resourceAndAmountDict = new Dictionary<ResourceEnum, uint>();
    }

    public void AddResourceStorage(ResourceStorage resourceStorage)
    {
        Debug.Log("added new resource storage area");
        resourceStorageList.Add(resourceStorage);
        if(resourceStorage.storedResourceType != ResourceEnum.Empty)
        {
            if (!resourceAndAmountDict.ContainsKey(resourceStorage.storedResourceType))
            {
                resourceAndAmountDict.Add(resourceStorage.storedResourceType, resourceStorage.currentAmountInInventory);
            }
        }
        resourceStorage.ResourceAddedHandler += ResourceWasAdded;
        resourceStorage.ResourceRemovedHandler += ResourceWasRemoved;
    }

    //FETCH LIST OF STORAGE AREAS WITH GIVEN RESOURCE TYPE
    public List<ResourceStorage> GetStorageAreas(ResourceEnum resourceEnum)
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
    public ResourceStorage GetClosestResourceStorageWithItem(in Vector2 pos, ResourceEnum resourceEnum)
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
    public ResourceStorage GetClosestResourceStorageToStoreItem(in Vector2 pos, ResourceEnum resourceEnum)
    {
        float dist = -1;
        ResourceStorage closestStorage = null;
        foreach (ResourceStorage resourceStorage in resourceStorageList)
        {
            if (resourceStorage.storedResourceType == resourceEnum || resourceStorage.storedResourceType == ResourceEnum.Empty)
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
    private void ResourceWasAdded(ResourceEnum resourceEnum, uint amnt)
    {
        resourceAndAmountDict[resourceEnum] += amnt;
    }

    //DETECT RESOURCE REMOVE
    private void ResourceWasRemoved(ResourceEnum resourceEnum, uint amnt)
    {
        //CAREFUL
        resourceAndAmountDict[resourceEnum] -= amnt;
    }
}

public enum ResourceEnum
{
    Empty,
    Stone,
    Wood,
    Food
}

