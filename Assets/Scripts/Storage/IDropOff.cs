using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDropOff 
{
    void DropOff(EntityType itemType, uint amount);
    Vector2Int GetLocation();
}
