using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile
{
    public TerrainTypes TerrainType { get; set; }
    public Sprite terrainSprite { get; set; }
    public bool IsTraversable { get; set; }
    public float TraversalDifficulty { get; set; }

    public Tile(TerrainTypes terrainType, Sprite terrainSprite, bool isTraversable, float traversalDifficulty)
    {
        TerrainType = terrainType;
        this.terrainSprite = terrainSprite;
        IsTraversable = isTraversable;
        TraversalDifficulty = traversalDifficulty;
    }
}
