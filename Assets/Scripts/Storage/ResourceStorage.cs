using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStorage
{
    public ResourceEnum storedResourceType { get; protected set; }
    public uint inventorySize { get; protected set; }
    public uint currentAmountInInventory { get; protected set; }
    public Vector2Int positionCellIndex { get; protected set; }

    //DELEGATES
    public event Action<ResourceEnum, uint> ResourceAddedHandler = delegate { }; 
    public event Action<ResourceEnum, uint> ResourceRemovedHandler = delegate { }; 

    public ResourceStorage(Vector2Int positionCellIndex)
    {
        this.positionCellIndex = positionCellIndex;
    }

    public ResourceStorage(ResourceEnum storedResource, uint inventorySize)
    {
        this.storedResourceType = storedResource;
        this.inventorySize = inventorySize;
    }

    public bool ChangeStoredResourceType(ResourceEnum newResourceType)
    {
        if(currentAmountInInventory == 0)
        {
            storedResourceType = newResourceType;
            return true;
        }
        else
        {
            return false;
        }
    }
    public uint GetSpaceLeft()
    {
        return inventorySize - currentAmountInInventory;
    }

    //ADDING ITEMS TO STORAGE RETURNS FALSE IF WAS NOT ABLE TO ADD ALL REQUESTED ITEMS
    public bool AddToStorage(ResourceEnum resourceType, ref uint addAmount)
    {
        if(storedResourceType == resourceType)
        {
            uint spaceLeft = inventorySize - currentAmountInInventory;
            int remaining = (int)(spaceLeft - addAmount);
            if(remaining >= 0)
            {
                currentAmountInInventory += addAmount;
                ResourceAddedHandler(resourceType, addAmount);
                return true;
            }
            else
            {
                currentAmountInInventory += spaceLeft;
                ResourceAddedHandler(resourceType, spaceLeft);
                addAmount = (uint)Mathf.Abs(remaining);
                return false;
            }
        }
        return false;
    }

    //REQUESTING ITEMS FROM STORAGE - RETURNS UINT OF ITEMS THAT CAN BE GRABBED
    public uint GrabResource(ResourceEnum requestedResourceType, uint requestedAmount)
    {
        if(storedResourceType == requestedResourceType)
        {
            if(requestedAmount <= currentAmountInInventory)
            {
                //return what was requested
                currentAmountInInventory -= requestedAmount;
                ResourceRemovedHandler(requestedResourceType, requestedAmount);
                return requestedAmount;
            }
            else
            {
                //return everything in the inventory
                currentAmountInInventory = 0;
                ResourceRemovedHandler(requestedResourceType, currentAmountInInventory);
                return currentAmountInInventory;
            }
        }
        return 0;
    }
}
