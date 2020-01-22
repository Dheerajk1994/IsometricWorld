using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEntity
{
    public string EntityName { get; private set; }
    public EntityType StaticEntityType { get; private set; }
    public Vector2Int CellIndex { get; private set; }
    public Sprite EntitySprite { get; private set; }

    public StaticEntity() { }

    public StaticEntity(string entityName, EntityType entityType, Vector2Int CellIndex, Sprite entitySprite)
    {
        this.EntityName = entityName;
        this.StaticEntityType = entityType;
        this.CellIndex = CellIndex;
        this.EntitySprite = entitySprite;
    }
}
