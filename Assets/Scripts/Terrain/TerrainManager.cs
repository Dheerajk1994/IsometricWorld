using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TerrainSpritesManager))]
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
    
    [SerializeField] public Sprite logsSprite;

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject entityPrefab;

    [SerializeField] private Transform tileParent;
    [SerializeField] private Transform entityParent;

    public int WorldWidth { get => worldWidth; }
    public int WorldHeight { get => worldHeight;  }
    public float TileScaleWidth { get => tileScaleWidth; }
    public float TileScaleHeight { get => tileScaleHeight; }
    public int TileUnitWidthSize { get => tileUnitWidthSize; }
    public int TileUnityHeightSize { get => tileUnityHeightSize;  }
    public int WorldOriginX { get => worldOriginX; }
    public int WorldOriginY { get => worldOriginY;  }

    private Tile[] tiles;               //array of tiles ie the terrain tiles    
    private Entity[] worldEntities;     //array of entity objects
    private GameObject[] worldObjects;  //array of entity game objects


    protected TerrainGenerator terrainGenerator;
    private TerrainSpritesManager terrainSpritesManager;

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
        worldEntities = new Entity[worldWidth * worldHeight];
        worldObjects = new GameObject[worldWidth * worldHeight];

        terrainSpritesManager = this.GetComponent<TerrainSpritesManager>();

        terrainGenerator.PopulateTerrainWithNoiseValues(ref tiles);
        terrainGenerator.PopulateTerrainWithEntities(tiles, ref worldEntities);

        terrainGenerator.DrawWorld(tiles, worldEntities, ref worldObjects, tilePrefab, entityPrefab, tileParent, entityParent);
    }

    protected void Update()
    {

    }
    
    //POSITION GETTERS/CONVERTERS
    public Vector2Int GetTilePosGivenWorldPos(float posX, float posY)
    {
        return terrainGenerator.GetTilePosAtPointer(posX, posY);
    }

    public Vector2Int GetTilePosGivenWorldPos(Vector2 pos)
    {
        return terrainGenerator.GetTilePosAtPointer(pos.x, pos.y);
    }

    public Vector2 GetWorldPosGivenTileIndex(Vector2Int tileIndex)
    {
        return terrainGenerator.GetTilePos(tileIndex.x, tileIndex.y);
    }

    public float GetDistanceBetween(in Vector2 pos1, in Vector2Int pos2)
    {
        return (Vector3.Distance(pos1, terrainGenerator.GetTilePos(pos2.x, pos2.y)));
    }

    //PATH REQUESTS
    public List<Vector2> RequestPath(Vector2 currentPos, Vector2 destinationPos)
    {
        //Debug.Log("requestpath to " + destinationPos);
        Vector2Int posStart = terrainGenerator.GetTilePosAtPointer(currentPos.x, currentPos.y);
        Vector2Int posEnd = terrainGenerator.GetTilePosAtPointer(destinationPos.x, destinationPos.y);
        List<Vector2Int> path = PathFinder.FindPath(ref tiles, ref worldWidth, ref worldHeight, ref posStart, ref posEnd);
        return terrainGenerator.TurnCellIndexesIntoPositions(path);
    }

    public List<Vector2> RequestPath(Vector2 currentPos, Vector2Int endCellIndex)
    {
        //Debug.Log("requestpath to cell " + endCellIndex);
        Vector2Int posStart = terrainGenerator.GetTilePosAtPointer(currentPos.x, currentPos.y);

        List<Vector2Int> path = PathFinder.FindPath(ref tiles, ref worldWidth, ref worldHeight, ref posStart, ref endCellIndex);
        return terrainGenerator.TurnCellIndexesIntoPositions(path);
    }

    //HELPERS
    public bool CanBeBuiltOn(in Vector2Int cellIndex)
    {
        return
            cellIndex.y * worldWidth + cellIndex.x >= 0 &&
            cellIndex.y * worldWidth + cellIndex.x < worldEntities.Length &&
            worldEntities[cellIndex.y * worldWidth + cellIndex.x] == null &&
            tiles[cellIndex.y * worldWidth + cellIndex.x].TerrainType != TerrainTypes.Water;
    }

    public StaticEntityType GetEntityTypeOn(in Vector2Int cellIndex)
    {
        if(worldEntities[cellIndex.y * worldWidth + cellIndex.x] != null)
        {
            return worldEntities[cellIndex.y * worldWidth + cellIndex.x].EntityType;
        }
        else
        {
            return StaticEntityType.Empty;
        }
    }

    public void DisplayTileAtPosition(ref GameObject tile, Vector2 pos)
    {
        terrainGenerator.PlaceTileInWorld(ref tile, pos);
    }

    public void AddBuildingToWorld(ConstructionObject constructionObj, Vector2Int arrayIndexPos)
    {
        worldEntities[arrayIndexPos.y * worldWidth + arrayIndexPos.x] = new Entity(constructionObj.buildingName, constructionObj.constructionObjectID, arrayIndexPos, constructionObj.buildingSprite);

        GameObject entity = Instantiate(entityPrefab);
        entity.transform.SetParent(entityParent);
        terrainGenerator.PlaceTileInWorld(ref entity, arrayIndexPos.x, arrayIndexPos.y);

        worldObjects[arrayIndexPos.y * worldWidth + arrayIndexPos.x] = entity;
        tiles[arrayIndexPos.y * worldWidth + arrayIndexPos.x].IsTraversable = constructionObj.traversable;
        tiles[arrayIndexPos.y * worldWidth + arrayIndexPos.x].TraversalDifficulty = constructionObj.traversalRate;

        //NEEDS REVISION
        if(constructionObj.constructionObjectID == StaticEntityType.Road)
        {
            terrainSpritesManager.UpdateRoadSprite(arrayIndexPos, worldWidth, worldHeight, ref entity, ref worldEntities, worldObjects);
        }
        else if(constructionObj.constructionObjectID == StaticEntityType.StorageArea)
        {
            ResourceManager.instance.AddResourceStorage(new ResourceStorage(arrayIndexPos));
            entity.GetComponent<SpriteRenderer>().sprite = constructionObj.buildingSprite;
        }
        else
        {
            entity.GetComponent<SpriteRenderer>().sprite = constructionObj.buildingSprite;
        }
    }

    public void RemoveEntityFromWorld(Vector2Int arrayIndexPos)
    {
        if(worldEntities[arrayIndexPos.y * worldWidth + arrayIndexPos.x] != null)
        {
            worldEntities[arrayIndexPos.y * worldWidth + arrayIndexPos.x] = null;
            Destroy(worldObjects[arrayIndexPos.y * worldWidth + arrayIndexPos.x].gameObject);
        }
    }

    public void AddEntityToWorld(Vector2Int pos, StaticEntityType type)
    {
        if(worldEntities[pos.y * WorldHeight + pos.x] == null)
        {
            worldEntities[pos.y * WorldHeight + pos.x] = new Entity(type.ToString(), type, pos, null);
            terrainGenerator.PlaceEntityInWorld(pos.x, pos.y, type, worldEntities, worldObjects, entityPrefab, entityParent);
        }
    }
}
