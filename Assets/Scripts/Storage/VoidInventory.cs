using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidInventory : IDropOff, IGrabFrom {

    Vector2Int location;

    public VoidInventory(Vector2Int location)
    {
        this.location = location;
    }

    public void DropOff(EntityType itemType, uint amount)
    {    }

    public Vector2Int GetLocation()
    {
        return location;
    }

    public int Grab(int amount)
    {
        return amount;
    }
}
