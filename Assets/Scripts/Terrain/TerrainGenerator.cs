﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TerrainGenerator
{
    #region PARAMETERS

    private int worldWidth, worldHeight;
    private float tileScaleWidth, tileScaleHeight;
    private int tileUnitWidthSize, tileUnityHeightSize;
    private int worldOriginX, worldOriginY;

    #endregion

    public TerrainGenerator(
        int worldWidth,
        int worldHeight,
        float tileScaleWidth,
        float tileScaleHeight, 
        int tileUnitWidthSize, 
        int tileUnityHeightSize,
        int worldOriginX, 
        int worldOriginY)
    {
        this.worldWidth = worldWidth;
        this.worldHeight = worldHeight;
        this.tileScaleWidth = tileScaleWidth;
        this.tileScaleHeight = tileScaleHeight;
        this.tileUnitWidthSize = tileUnitWidthSize;
        this.tileUnityHeightSize = tileUnityHeightSize;
        this.worldOriginX = worldOriginX;
        this.worldOriginY = worldOriginY;
    }

    public Tuple<int, int> GetTilePosAtPointer(float worldPosX, float worldPosY)
    {
        int cellX = Mathf.RoundToInt(worldPosX / tileScaleWidth);
        int cellY = Mathf.RoundToInt(worldPosY / tileScaleHeight);

        int selectedX = Mathf.RoundToInt((cellY - worldOriginY) + (cellX - worldOriginX));
        int selectedY = Mathf.RoundToInt((cellY - worldOriginY) - (cellX - worldOriginX));

        float offsetX;
        float offsetY;

        Vector2 pos = GetTilePos(selectedX, selectedY);

        offsetX = worldPosX - pos.x;
        offsetY = worldPosY - pos.y;

        //Debug.Log("x: " + offsetX + " y" + offsetY);

        if (Mathf.Abs(offsetX) + 2 * Mathf.Abs(offsetY) > tileScaleHeight)
        {
            if (offsetX > 0 && offsetY > 0)
            {
                selectedX++;
            }
            else if (offsetX > 0 && offsetY < 0)
            {
                selectedY--;
            }
            else if (offsetX < 0 && offsetY > 0)
            {
                selectedY++;
            }
            else
            {
                selectedX--;
            }
        }
        return Tuple.Create(selectedX, selectedY);
    }

    public void PopulateTerrainWithNoiseValues(ref Tile[] tiles)
    {
        for (int y = 0; y < worldHeight; ++y)
        {
            for (int x = 0; x < worldWidth; ++x)
            {
                float terrainVal =TerrainNoise.GetNoise(x, y, 10f);
                //needs optimization 
                if (terrainVal < 0f)
                {
                    tiles[worldHeight * y + x] = new Tile(TerrainTypes.Water, null, false, 0);
                }
                else if (terrainVal < 0.35f)
                {
                    tiles[worldHeight * y + x] = new Tile(TerrainTypes.Sand, null, true, 1);
                }
                else
                {
                    tiles[worldHeight * y + x] = new Tile(TerrainTypes.Plains, null, true, 1);
                }
            }
        }
    }

    public void PopulateTerrainWithEntities(in Tile[] tiles, ref Entity[] entities)
    {
        for (int x = 0; x < worldWidth; ++x)
        {
            for (int y = 0; y < worldHeight; ++y)
            {
                float entityVal = TerrainNoise.GetNoise(x, y, 50f);
                if (entityVal < 0.3f && tiles[y * worldHeight + x].TerrainType != TerrainTypes.Water)
                {
                    entities[y * worldHeight + x] = new Entity("Tree", StaticEntityType.Tree, new Tuple<int, int>(x, y), null);
                }
                else if(entityVal > 0.5f && entityVal < 0.52f && tiles[y * worldHeight + x].TerrainType != TerrainTypes.Water)
                {
                    entities[y * worldHeight + x] = new Entity("Stone", StaticEntityType.Stone, new Tuple<int, int>(x, y), null);
                }
            }
        }
    }

    public void DrawWorld(in Tile[] tiles, in Entity[] entities, GameObject tilePrefab, GameObject entityPrefab, in Transform tilesParent, in Transform entityParent)
    {
        for(int y = 0; y < worldHeight; ++y)
        {
            for (int x = 0; x < worldWidth; ++x)
            {
                //TILE
                if(tiles[y * worldHeight + x] != null)
                {
                    GameObject tile = UnityEngine.Object.Instantiate(tilePrefab);
                    tile.transform.SetParent(tilesParent);
                    SetTerrainSprite(tiles[y * worldHeight + x].TerrainType, ref tile);
                    PlaceTileInWorld(ref tile, x, y);
                }

                //ENTITY
                if(entities[y * worldHeight + x] != null)
                {
                    GameObject entity = UnityEngine.Object.Instantiate(entityPrefab);
                    entity.transform.SetParent(entityParent);
                    SetEntitySprite(entities[y * worldHeight + x].EntityType, ref entity);
                    PlaceTileInWorld(ref entity, x, y);
                }
            }
        }
    }

    public void SetTerrainSprite(in TerrainTypes type, ref GameObject tile)
    {
        //needs optimization 
        switch (type)
        {
            case TerrainTypes.Water:
                tile.GetComponent<SpriteRenderer>().sprite = TerrainManager.instance.waterSprite;
                break;
            case TerrainTypes.Sand:
                tile.GetComponent<SpriteRenderer>().sprite = TerrainManager.instance.sandSprite;
                break;
            case TerrainTypes.Plains:
                tile.GetComponent<SpriteRenderer>().sprite = TerrainManager.instance.plainsSprite;
                break;
            default:
                break;
        }
    }

    public void SetEntitySprite(StaticEntityType type, ref GameObject entity)
    {
        //needs optimization 
        switch (type)
        {
            case StaticEntityType.Tree:
                entity.GetComponent<SpriteRenderer>().sprite = TerrainManager.instance.treeSprite;
                break;
            case StaticEntityType.Stone:
                entity.GetComponent<SpriteRenderer>().sprite = TerrainManager.instance.stoneSprite;
                break;
        }
    }

    public void PlaceTileInWorld(ref GameObject tile, int x, int y)
    {
        tile.transform.position = GetTilePos(x, y);
    }

    public void PlaceTileInWorld(ref GameObject tile, Vector2 position)
    {
        Tuple<int, int> pos = GetTilePosAtPointer(position.x, position.y);
        PlaceTileInWorld(ref tile, pos.Item1, pos.Item2);
    }

    //HELPER FUNCTIONS

    public List<Vector2> TurnCellIndexesIntoPositions(in List<Tuple<int, int>> path)
    {
        if (path == null) { return null; }
        List<Vector2> newPath = new List<Vector2>();
        foreach (Tuple<int, int> tuple in path)
        {
            newPath.Add(GetTilePos(tuple.Item1, tuple.Item2));
        }
        return newPath;
    }

    public bool IsCoordInWorld(int x, int y)
    {
        return (x >= 0 && x < worldWidth && y >= 0 && y < worldHeight);
    }

    public Vector2 GetTilePos(int x, int y)
    {
        return new Vector2(
            (worldOriginX * tileScaleWidth) + (x - y) * tileScaleWidth / 2,
            (worldOriginY * tileScaleHeight) + (x + y) * tileScaleHeight / 2);
    }

}