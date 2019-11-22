using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;
    [SerializeField]private List<ResourceStorage> storageAreas;

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

    private void Start()
    {
        if(storageAreas == null)
        {
            storageAreas = new List<ResourceStorage>();
        }
    }

    //YOU NEED SERIOUS HELP
    public Vector2 getStorageAreaLocation(ResourceEnum resourceEnum)
    {
        return storageAreas[0].position;
    }
}

public enum ResourceEnum
{
    Stone,
    Wood
}

