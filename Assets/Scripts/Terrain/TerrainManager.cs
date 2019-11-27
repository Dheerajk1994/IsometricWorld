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

        terrainGenerator.DrawWorld(tiles, worldEntities, tilePrefab, entitiesPrefab, tilesParent, gameEntitiesParent);
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

    public float GetDistanceBetween(in Vector2 pos1, in Tuple<int, int> pos2)
    {
        return (Vector3.Distance(pos1, terrainGenerator.GetTilePos(pos2.Item1, pos2.Item2)));
    }

    //PATH REQUESTS
    public List<Vector2> RequestPath(Vector2 currentPos, Vector2 destinationPos)
    {
        //Debug.Log("requestpath to " + destinationPos);
        Tuple<int, int> posStart = terrainGenerator.GetTilePosAtPointer(currentPos.x, currentPos.y);
        Tuple<int, int> posEnd = terrainGenerator.GetTilePosAtPointer(destinationPos.x, destinationPos.y);
        List<Tuple<int, int>> path = PathFinder.FindPath(ref tiles, ref worldWidth, ref worldHeight, ref posStart, ref posEnd);
        return terrainGenerator.TurnCellIndexesIntoPositions(path);
    }

    public List<Vector2> RequestPath(Vector2 currentPos, Tuple<int, int> endCellIndex)
    {
        //Debug.Log("requestpath to cell " + endCellIndex);
        Tuple<int, int> posStart = terrainGenerator.GetTilePosAtPointer(currentPos.x, currentPos.y);

        List<Tuple<int, int>> path = PathFinder.FindPath(ref tiles, ref worldWidth, ref worldHeight, ref posStart, ref endCellIndex);
        return terrainGenerator.TurnCellIndexesIntoPositions(path);
    }

    //HELPERS
    public bool CanBeBuiltOn(in Tuple<int, int> cellIndex)
    {
        return
            cellIndex.Item2 * worldWidth + cellIndex.Item1 >= 0 &&
            cellIndex.Item2 * worldWidth + cellIndex.Item1 < worldEntities.Length &&
            worldEntities[cellIndex.Item2 * worldWidth + cellIndex.Item1] == null &&
            tiles[cellIndex.Item2 * worldWidth + cellIndex.Item1].TerrainType != TerrainTypes.Water;
    }

    public void DisplayTileAtPosition(ref GameObject tile, Vector2 pos)
    {
        terrainGenerator.PlaceTileInWorld(ref tile, pos);
    }

    public void AddBuildingToWorld(ConstructionObject constructionObj, Tuple<int, int> arrayIndexPos)
    {
        worldEntities[arrayIndexPos.Item2 * worldWidth + arrayIndexPos.Item1] = new Entity(constructionObj.buildingName, constructionObj.constructionObjectID, arrayIndexPos, constructionObj.buildingSprite);

        GameObject entity = Instantiate(entitiesPrefab);
        entity.transform.SetParent(gameEntitiesParent);
        terrainGenerator.PlaceTileInWorld(ref entity, arrayIndexPos.Item1, arrayIndexPos.Item2);

        worldObjects[arrayIndexPos.Item2 * worldWidth + arrayIndexPos.Item1] = entity;
        tiles[arrayIndexPos.Item2 * worldWidth + arrayIndexPos.Item1].IsTraversable = constructionObj.traversable;
        tiles[arrayIndexPos.Item2 * worldWidth + arrayIndexPos.Item1].TraversalDifficulty = constructionObj.traversalRate;

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


    public void RemoveBuildingFromWorld(Tuple <int, int> arrayIndexPos)
    {
        if(worldEntities[arrayIndexPos.Item1 * worldWidth + arrayIndexPos.Item2] != null)
        {
            worldEntities[arrayIndexPos.Item1 * worldWidth + arrayIndexPos.Item2] = null;
            Destroy(worldObjects[arrayIndexPos.Item1 * worldWidth + arrayIndexPos.Item2].gameObject);
        }
    }
}
