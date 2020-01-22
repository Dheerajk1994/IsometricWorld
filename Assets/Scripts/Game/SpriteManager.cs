using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance;
    //tile sprites
    [SerializeField] private Sprite[] water;
    [SerializeField] private Sprite[] grass;
    [SerializeField] private Sprite[] sand;

    //static entities
    [SerializeField] private Sprite[] tree_pine;
    [SerializeField] private Sprite[] boulder_stone;

    [SerializeField] private Sprite[] tent;
    [SerializeField] private Sprite[] house;
    [SerializeField] private Sprite[] storageArea;
    [SerializeField] private Sprite[] road;
    
    [SerializeField] private Sprite[] logs;
    [SerializeField] private Sprite[] stone;

    void Awake()
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

    public Sprite GetTerrainSprite(TerrainTypes type)
    {
        switch (type)
        {
            case TerrainTypes.Water:
                return water[UnityEngine.Random.Range(0, water.Length)];
            case TerrainTypes.Sand:
                return sand[UnityEngine.Random.Range(0, sand.Length)];
            case TerrainTypes.Plains:
                return grass[UnityEngine.Random.Range(0, grass.Length)];
            default:
                Debug.LogError("Unknown terrain type requested. Type " + type);
                return null;
        }
    }

    public Sprite GetStaticEntitySprite(EntityType type)
    {
        switch (type)
        {
            case EntityType.Empty:
                return null;
            case EntityType.Tree_Pine:
                return tree_pine[UnityEngine.Random.Range(0, tree_pine.Length)];
            case EntityType.Boulder_Stone:
                return boulder_stone[UnityEngine.Random.Range(0, boulder_stone.Length)];
            case EntityType.Tent:
                return tent[UnityEngine.Random.Range(0, tent.Length)];
            case EntityType.House:
                return house[UnityEngine.Random.Range(0, house.Length)];
            case EntityType.StorageArea:
                return storageArea[UnityEngine.Random.Range(0, storageArea.Length)];
            case EntityType.Road:
                return road[UnityEngine.Random.Range(0, road.Length)];
            case EntityType.Logs:
                return logs[UnityEngine.Random.Range(0, logs.Length)];
            case EntityType.Stone:
                return stone[UnityEngine.Random.Range(0, stone.Length)];
            default:
                Debug.LogError("Unknown static entity type requested. Type " + type);
                return null;
        }
    }
}
