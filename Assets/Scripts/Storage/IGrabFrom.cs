using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  interface IGrabFrom
{
    int Grab(int amount);
    Vector2Int GetLocation();
}
