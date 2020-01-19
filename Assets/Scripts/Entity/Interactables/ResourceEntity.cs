using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceEntity : Entity, IGrabFrom
{
    public ResourceEntity(string entityName, StaticEntityType entityType, Vector2Int CellIndex, Sprite entitySprite) 
        :
        base(entityName, entityType, CellIndex, entitySprite)
    {}

    public uint resourceAmount { get; protected set; }

    public void Grab(StaticEntityType itemType, int amount)
    {
        Debug.LogError("grabbed from resource entity");
    }
}
