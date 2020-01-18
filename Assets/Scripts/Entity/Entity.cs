using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    public string EntityName { get; private set; }
    public StaticEntityType EntityType { get; private set; }
    public Vector2Int CellIndex { get; private set; }
    public Sprite EntitySprite { get; private set; }

    public Entity() { }

    public Entity(string entityName, StaticEntityType entityType, Vector2Int CellIndex, Sprite entitySprite)
    {
        this.EntityName = entityName;
        this.EntityType = entityType;
        this.CellIndex = CellIndex;
        this.EntitySprite = entitySprite;
    }
}
