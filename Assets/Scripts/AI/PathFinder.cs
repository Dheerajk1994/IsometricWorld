using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//g distance so far from the starting point
//h guess to the end point from current node
//f = g + h

public static class PathFinder
{
    static TileVal[] tiles;
    static List<TileVal> openList;
    static int destinationTileX, destinationTileY;


    public static List<Tuple<int, int>> FindPath(ref Tile[] worldTileTraversalData, 
                                  ref int worldWidth, 
                                  ref int worldHeight, 
                                  ref Tuple<int, int> startTile,
                                  ref Tuple<int, int> destTile)
    {
        int startingTileX = startTile.Item1;
        int startingTileY = startTile.Item2;

        destinationTileX = destTile.Item1;
        destinationTileY = destTile.Item2;

        tiles = new TileVal[worldWidth * worldHeight];
        openList = new List<TileVal>();

        InitializeTiles(ref worldTileTraversalData, ref worldWidth, ref worldHeight);

        //add starting tile into openlist
        openList.Add(tiles[worldWidth * startingTileY + startingTileX]);
        tiles[worldWidth * startingTileY + startingTileX].isInOpenList = true;

        int loopCount = 0;

        //while openlist is not empty
        while(openList.Count > 0)
        {
            //q = node with least f in open list
            TileVal q = GetTileWithLeastFInOpenList();

            //pop q off the open list
            openList.Remove(q);

            //generate 8 successors of q and set their parent to q
            if(GetNeighbors(ref q, ref worldWidth, ref worldHeight))
            {
                //stop and return path
                //Debug.Log("found path in while : at loop " + loopCount);
                return GetFinalPath(openList[openList.Count - 1], tiles[startingTileY * worldHeight + startingTileX]);
            }
            q.isClosed = true;
            loopCount++;
            if(loopCount > 2000)
            {
                Debug.Log("endless loop detected");
                return null;
            }
        }
        Debug.Log("no path");
        return null;
    }

    //HELPER FUNCTIONS

    private static void InitializeTiles(ref Tile[] worldTileTraversalData, ref int worldWidth, ref int worldHeight)
    {
        for(int x = 0; x < worldWidth; ++x)
        {
            for (int y = 0; y < worldHeight; ++y)
            {
                tiles[y * worldWidth + x] = new TileVal();
                tiles[y * worldWidth + x].isWall = !worldTileTraversalData[y * worldWidth + x].IsTraversable;
                tiles[y * worldWidth + x].traversalRate = worldTileTraversalData[y * worldWidth + x].TraversalDifficulty;
                tiles[y * worldWidth + x].x = x;
                tiles[y * worldWidth + x].y = y;
            }
        }
    }

    private static TileVal GetTileWithLeastFInOpenList()
    {
        TileVal tile = openList[0];
        float leastF = tile.f;
        foreach (TileVal t in openList)
        {
            if(t.f < leastF)
            {
                tile = t;
                leastF = t.f;
            }
        }
        return tile;
    }

    private static bool GetNeighbors(ref TileVal currentTile, ref int worldWidth, ref int worldHeight)
    {
        //NEEDS OPTIMIZATION
        int cX = currentTile.x;
        int cY = currentTile.y;
        int indexOfDestination = destinationTileY * worldHeight + destinationTileX;
        if (cX > 0 && cY > 0 && cX < worldWidth - 1 && cY < worldHeight - 1)
        {
            //middle left
            if (AddPotentionNeighbor((cY) * worldHeight + (cX - 1), ref currentTile, ref indexOfDestination))
            {
                return true; 
            }
            //bottom middle
            if (AddPotentionNeighbor((cY - 1) * worldHeight + (cX), ref currentTile, ref indexOfDestination))
            {
                return true; 
            }
            //top middle
            if (AddPotentionNeighbor((cY + 1) * worldHeight + (cX), ref currentTile, ref indexOfDestination))
            {
                return true; 
            }

            //middle right
            if (AddPotentionNeighbor((cY) * worldHeight + (cX + 1), ref currentTile, ref indexOfDestination))
            {
                return true;
            }

            //CORNERS 

            //bottom left
            if(AddPotentionNeighbor((cY - 1) * worldHeight + (cX - 1), ref currentTile, ref indexOfDestination))
            {
                return true;
            }
            
            
            //top left
            if (AddPotentionNeighbor((cY + 1) * worldHeight + (cX - 1), ref currentTile, ref indexOfDestination))
            {
                return true; 
            }
            
            
            //bottom right
            if (AddPotentionNeighbor((cY - 1) * worldHeight + (cX + 1), ref currentTile, ref indexOfDestination))
            {
                return true; 
            }
            
            ////top right
            if (AddPotentionNeighbor((cY + 1) * worldHeight + (cX + 1), ref currentTile, ref indexOfDestination))
            {
                return true; 
            }
        }

        return false;
    }

    private static bool AddPotentionNeighbor(int indexToCheck, ref TileVal parent, ref int indexOfDestination)
    {
        if (!tiles[indexToCheck].isClosed)
        {
            //NEED WORK
            if (tiles[indexToCheck].isWall || tiles[indexToCheck].isInOpenList)
                return false;
            openList.Add(tiles[indexToCheck]);
            tiles[indexToCheck].isInOpenList = true;
            tiles[indexToCheck].parentile = parent;
            //TRAVERSAL RATE
            tiles[indexToCheck].g = parent.g + tiles[indexToCheck].traversalRate;
            tiles[indexToCheck].h = HeurDistanceToEndGoal(ref indexToCheck, ref indexOfDestination);
            tiles[indexToCheck].UpdateF();
            if (tiles[indexToCheck] == tiles[indexOfDestination])
            {
                return true;
            }
        }
        return false;
    }

    private static float HeurDistanceToEndGoal(ref int currentIndex, ref int indexOfDestination)
    {
        //OUT OF RANGE WARNINGS HERE
        try
        {
            return (Mathf.Sqrt(Mathf.Pow((tiles[currentIndex].x - tiles[indexOfDestination].x), 2) +
                               Mathf.Pow((tiles[currentIndex].y - tiles[indexOfDestination].y), 2)));
        }
        catch(Exception e)
        {
            Debug.Log(currentIndex + " " + indexOfDestination);
            Debug.Log(e.Message);
        }
        return 0;        
    }

    private static List<Tuple<int, int>> GetFinalPath(TileVal endTile, TileVal startingTile)
    {
        List<Tuple<int, int>> path = new List<Tuple<int, int>>();
        TileVal currentTile = endTile;
        while(currentTile != startingTile)
        {
            path.Add(Tuple.Create(currentTile.x, currentTile.y));
            currentTile = currentTile.parentile;
        }
        path.Reverse();
        return path;
    }
}

public class TileVal : IEquatable<TileVal>
{
    public TileVal parentile = null;
    public int x, y;
    public bool isClosed;
    public bool isInOpenList;
    public bool isWall;
    public float traversalRate;
    public float g;
    public float h;
    public float f;

    public bool Equals(TileVal other)
    {
        return (other.x == this.x && other.y == this.y);
    }

    public void UpdateF()
    {
        this.f = this.g + this.h;
    }
}