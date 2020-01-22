using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTree : ResourceEntity, IChoppable
{
    public EntityTree(string entityName, EntityType entityType, Vector2Int CellIndex, Sprite entitySprite) : base(entityName, entityType, CellIndex, entitySprite)
    {
    }

    public void Chop()
    {
        throw new System.NotImplementedException();
    }
}
