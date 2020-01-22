using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathFinder
{
    public static List<Vector2Int> FindPath(
                                    in Tile[] worldTileTraversalData, 
                                    in int worldWidth, 
                                    in int worldHeight, 
                                    in Vector2Int startTile,
                                    in Vector2Int destTile)
    {
        if (startTile == destTile)
        {
            List<Vector2Int> path = new List<Vector2Int> { startTile };
            return path;
        }

        TileVal[] tiles = new TileVal[worldWidth * worldHeight];

        for(int x = 0; x < worldWidth; ++x)
        {
            for (int y = 0; y < worldHeight; ++y)
            {
                tiles[worldWidth * y + x] = new TileVal(x, y);
            }
        }

        TileVal source = tiles[worldWidth * startTile.y + startTile.x];
        TileVal destination = tiles[worldWidth * destTile.y + destTile.x];

        List<TileVal> openList = new List<TileVal>();
        openList.Add(source);
        source.open = true;

        TileVal q;
        
        int loopCount = 0;

        //while openlist is not empty
        while(openList.Count > 0 && ++loopCount < 50000)
        {
            openList.Sort((a, b) => a.f.CompareTo(b.f));
            q = openList[0];
            List<TileVal> neighbors = GetNeighbors(q, tiles, worldWidth, worldHeight);
            foreach(TileVal n in neighbors)
            {
                if(n == destination)
                {
                    Debug.Log("path found in " + loopCount + " iterations");
                    n.parentile = q;
                    return GetFinalPath(q, source);
                }
                if(n.closed == false && n.open == false && worldTileTraversalData[worldWidth * n.y + n.x].IsTraversable)
                {
                    n.parentile = q;
                    n.g = q.g + worldTileTraversalData[worldWidth * n.y + n.x].TraversalDifficulty;
                    n.h = CalculateHValue(n, destination);
                    n.f = n.g + n.h;
                    openList.Add(n);
                    n.open = true;
                }
            }
            q.closed = true;
            openList.RemoveAt(0);
            q.open = false;
        }
        Debug.Log("no path from " + startTile + " to " + destTile);
        return null;
    }


    private static List<TileVal> GetNeighbors(in TileVal currentTile, in TileVal[] tiles, in int worldWidth, in int worldHeight)
    {
        List<TileVal> neighbors = new List<TileVal>();
        if (currentTile.x == 0 || currentTile.y == 0 || currentTile.x >= worldWidth - 2 || currentTile.y >= worldHeight - 2)
            return neighbors;
        int x = currentTile.x;
        int y = currentTile.y;

        //left
        neighbors.Add(tiles[worldWidth * (y - 1) + (x - 1)]);
        neighbors.Add(tiles[worldWidth * (y    ) + (x - 1)]);
        neighbors.Add(tiles[worldWidth * (y + 1) + (x - 1)]);
        //middle
        neighbors.Add(tiles[worldWidth * (y - 1) + (x    )]);
        neighbors.Add(tiles[worldWidth * (y + 1) + (x    )]);
        //right
        neighbors.Add(tiles[worldWidth * (y - 1) + (x + 1)]);
        neighbors.Add(tiles[worldWidth * (y    ) + (x + 1)]);
        neighbors.Add(tiles[worldWidth * (y + 1) + (x + 1)]);

        return neighbors;
    }

    private static float CalculateHValue(TileVal successor, in TileVal destTile)
    {
        return Mathf.Sqrt(
            Mathf.Pow(successor.x - destTile.x, 2) +
            Mathf.Pow(successor.y - destTile.y, 2));
    }

    private static List<Vector2Int> GetFinalPath(in TileVal endTile, in TileVal startingTile)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        TileVal currentTile = endTile;
        while(currentTile != startingTile)
        {
            path.Add(new Vector2Int(currentTile.x, currentTile.y));
            currentTile = currentTile.parentile;
        }
        path.Add(new Vector2Int(startingTile.x, startingTile.y));
        path.Reverse();
        return path;
    }
}

public class TileVal : IEquatable<TileVal>
{
    public TileVal(int x, int y)
    {
        this.x = x;
        this.y = y;
        g = 0;
        h = 0;
        f = 0;
        closed = false;
    }
    public TileVal parentile = null;
    public int x, y;
    public float g;
    public float h;
    public float f;
    public bool closed = false;
    public bool open = false;

    public bool Equals(TileVal other)
    {
        return (other.x == this.x && other.y == this.y);
    }
}