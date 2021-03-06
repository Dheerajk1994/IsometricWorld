﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceEntity : StaticEntity, IGrabFrom
{
    public ResourceEntity(string entityName, EntityType entityType, Vector2Int CellIndex, Sprite entitySprite) 
        :
        base(entityName, entityType, CellIndex, entitySprite)
    {}

    public uint resourceAmount { get; protected set; }

    public Vector2Int GetLocation()
    {
        return CellIndex;
    }

    public int Grab(int amount)
    {
        TerrainManager.instance.RemoveEntityFromWorld(CellIndex);
        return amount;
    }
}
