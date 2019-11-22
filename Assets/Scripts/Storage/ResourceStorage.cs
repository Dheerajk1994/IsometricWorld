using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
    private Dictionary<ResourceEnum, int> storedResources;
    public Vector2 position { get; private set; }

    void Start()
    {
        storedResources = new Dictionary<ResourceEnum, int>();
        this.position = this.transform.position;
    }

    public int GrabResource(ResourceEnum resource, uint amnt)
    {
        return 10;
    }
}
