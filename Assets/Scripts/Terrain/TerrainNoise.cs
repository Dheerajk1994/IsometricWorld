using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainNoise
{
    public static float GetNoise(in int x, in int y, in float seed)
    {
        float xF = x * 0.1f + seed;
        float yF = y * 0.1f + seed; 
        return Mathf.PerlinNoise(xF, yF);
    }
}
