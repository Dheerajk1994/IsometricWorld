using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainManager : MonoBehaviour
{
    public static TerrainManager instance;

    [SerializeField] private int worldWidth, worldHeight;
    [SerializeField] private float tileScaleWidth, tileScaleHeight;
    [SerializeField] private int tileUnitWidthSize, tileUnityHeightSize;
    [SerializeField] private int worldOriginX, worldOriginY;

    [SerializeField] public Sprite waterSprite;
    [SerializeField] public Sprite sandSprite;
    [SerializeField] public Sprite plainsSprite;

    [SerializeField] public Sprite treeSprite;
    [SerializeField] public Sprite tentSprite;
    [SerializeField] public Sprite houseSprite;
    [SerializeField] public Sprite stoneSprite;

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject entitiesPrefab;

    [SerializeField] private Transform tilesParent;
    [SerializeField] private Transform gameEntitiesParent;

    public int WorldWidth { get => worldWidth; }
    public int WorldHeight { get => worldHeight;  }
    public float TileScaleWidth { get => tileScaleWidth; }
    public float TileScaleHeight { get => tileScaleHeight; }
    public int TileUnitWidthSize { get => tileUnitWidthSize; }
    public int TileUnityHeightSize { get => tileUnityHeightSize;  }
    public int WorldOriginX { get => worldOriginX; }
    public int WorldOriginY { get => worldOriginY;  }

    private Tile[] tiles;
    private Entity[] worldObjects;


    protected TerrainGenerator terrainGenerator;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(this != instance)
        {
            Destroy(this);
        }
    }

    protected void Start()
    {
        terrainGenerator = new TerrainGenerator(
            WorldWidth, 
            WorldHeight,
            TileScaleWidth,
            TileScaleHeight, 
            TileUnitWidthSize, 
            TileUnityHeightSize, 
            WorldOriginX, 
            WorldOriginY);

        tiles = new Tile[worldWidth * worldHeight];
        worldObjects = new Entity[worldWidth * worldHeight];

        terrainGenerator.PopulateTerrainWithNoiseValues(ref tiles);
        terrainGenerator.PopulateTerrainWithEntities(tiles, ref worldObjects);

        terrainGenerator.DrawWorld(tiles, worldObjects, tilePrefab, entitiesPrefab, tilesParent, gameEntitiesParent);
    }

    protected void Update()
    {

    }
    
    //POSITION GETTERS/CONVERTERS
    public Tuple<int, int> GetTilePosGivenWorldPos(float posX, float posY)
    {
        return terrainGenerator.GetTilePosAtPointer(posX, posY);
    }

    public Tuple<int, int> GetTilePosGivenWorldPos(Vector2 pos)
    {
        return terrainGenerator.GetTilePosAtPointer(pos.x, pos.y);
    }

    public Vector2 GetWorldPosGivenTileIndex(Tuple<int, int> tileIndex)
    {
        return terrainGenerator.GetTilePos(tileIndex.Item1, tileIndex.Item2);
    }

    //PATH REQUESTS
    public List<Vector2> RequestPath(Vector2 currentPos, Vector2 destinationPos)
    {
        Debug.Log("requestpath to " + destinationPos);
        Tuple<int, int> posStart = terrainGenerator.GetTilePosAtPointer(currentPos.x, currentPos.y);
        Tuple<int, int> posEnd = terrainGenerator.GetTilePosAtPointer(Mathf.Clamp(destinationPos.x, 2, worldWidth - 2),
                                                                      Mathf.Clamp(destinationPos.y, 2, worldHeight - 2));
        List<Tuple<int, int>> path = PathFinder.FindPath(ref tiles, ref worldWidth, ref worldHeight, ref posStart, ref posEnd);
        return terrainGenerator.TurnCellIndexesIntoPositions(path);
    }

    public List<Vector2> RequestPath(Vector2 currentPos, Tuple<int, int> endCellIndex)
    {
        Debug.Log("requestpath to cell " + endCellIndex);
        Tuple<int, int> posStart = terrainGenerator.GetTilePosAtPointer(currentPos.x, currentPos.y);

        List<Tuple<int, int>> path = PathFinder.FindPath(ref tiles, ref worldWidth, ref worldHeight, ref posStart, ref endCellIndex);
        return terrainGenerator.TurnCellIndexesIntoPositions(path);
    }

    //
    public bool CanBeBuiltOn(ref Vector2 buildPos)
    {
        return true;
    }

    public void DisplayTileAtPosition(ref GameObject tile, Vector2 pos)
    {
        terrainGenerator.PlaceTileInWorld(ref tile, pos);
    }

    public void AddBuildingToWorld(ConstructionObject building, Tuple<int, int> arrayIndexPos)
    {
        worldObjects[arrayIndexPos.Item1 * worldWidth + arrayIndexPos.Item2] = new Entity(building.buildingName, building.constructionObjectID, arrayIndexPos, building.buildingSprite);

        GameObject entity = Instantiate(entitiesPrefab);
        entity.transform.SetParent(gameEntitiesParent);
        terrainGenerator.PlaceTileInWorld(ref entity, arrayIndexPos.Item1, arrayIndexPos.Item2);
        entity.GetComponent<SpriteRenderer>().sprite = building.buildingSprite;

        tiles[arrayIndexPos.Item1 * worldWidth + arrayIndexPos.Item2].IsTraversable = false;
    }
}
