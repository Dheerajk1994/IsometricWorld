using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResourceEntity : Entity
{
    public ResourceEnum resourceType { get; protected set; }
    public uint resourceAmount { get; protected set; }
}
