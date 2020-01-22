using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainNoise
{
    private Vector2Int worldSize;

    public TerrainNoise(Vector2Int worldSize)
    {
        this.worldSize = worldSize;
    }

    public float GetNoise(in int x, in int y, in float seed)
    {
        float distX = Mathf.Abs(x - worldSize.x * 0.5f);
        float distY = Mathf.Abs(y - worldSize.y * 0.5f);
        float dist = Mathf.Sqrt(distX * distX + distY * distY);

        float max_width = worldSize.x * 0.5f - 2.0f;
        float delta = dist / max_width;
        float gradient = delta * delta;

        float xF = x * 0.1f + seed;
        float yF = y * 0.1f + seed; 

        float noise = Mathf.PerlinNoise(xF, yF);
        return (noise * (Mathf.Max(0.0f, 1.0f - gradient)));
    }
}
