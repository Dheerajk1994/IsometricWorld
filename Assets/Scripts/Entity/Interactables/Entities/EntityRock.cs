using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRock : ResourceEntity, IMineable
{
    public EntityRock(ResourceEnum resourceEnum, uint resourceAmount, string entityName, StaticEntityType entityType, Tuple<int, int> CellIndex, Sprite entitySprite) : base()
    {
        
    }

    public void Mine()
    {
        throw new System.NotImplementedException();
    }
}
