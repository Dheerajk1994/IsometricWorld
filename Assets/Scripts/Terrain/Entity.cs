using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    public string EntityName { get; private set; }
    public StaticEntityType EntityType { get; private set; }
    public Tuple<int, int> CellIndex { get; private set; }
    public Sprite EntitySprite { get; private set; }

    public Entity() { }

    public Entity(string entityName, StaticEntityType entityType, Tuple<int, int> CellIndex, Sprite entitySprite)
    {
        this.EntityName = entityName;
        this.EntityType = entityType;
        this.CellIndex = CellIndex;
        this.EntitySprite = entitySprite;
    }
}
