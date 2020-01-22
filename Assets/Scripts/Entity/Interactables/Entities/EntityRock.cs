using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRock : ResourceEntity, IMineable
{
    public EntityRock(string entityName, EntityType entityType, Vector2Int CellIndex, Sprite entitySprite) : base(entityName, entityType, CellIndex, entitySprite)
    {
    }

    public void Mine()
    {
        throw new System.NotImplementedException();
    }
}
