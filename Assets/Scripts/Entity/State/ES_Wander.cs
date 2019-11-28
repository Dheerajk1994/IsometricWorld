using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ES_Wander : IEntityState
{
    GameObject entity;
    EntityMovement entityMovement;
    private int maxWanderRange;
    private Vector2 moveAroundArea;

    private readonly float timeToWaitUntilNextWanderRequest = 3f;
    private float currentWaitTime = 0f;

    private bool isWandering = false;

    //CONSTRUCTOR
    public ES_Wander(GameObject entity, int maxWanderRange, Vector2 moveAroundArea)
    {
        this.entity = entity;
        this.maxWanderRange = maxWanderRange;
        this.moveAroundArea = moveAroundArea;

        entityMovement = entity.GetComponent<EntityMovement>();
        entityMovement.DestinationReachedHandler += HasArrivedAtDestination;
    }

    //INTERFACE IMPLEMENTATION

    public void Enter()
    {
        currentWaitTime = 0f;
        isWandering = false;
    }

    public void Execute()
    {
        if(!isWandering)
        {
            if(currentWaitTime >= timeToWaitUntilNextWanderRequest)
            {
                entityMovement.Move(TerrainManager.instance.RequestPath(entity.transform.position, GetRandomLocationAroundArea()));
                isWandering = true;
                currentWaitTime = 0f;
            }
            else
            {
                currentWaitTime += Time.deltaTime;
            }
        }
    }

    public void Stop()
    {
    }

    public bool WillStop()
    {
        return true;
    }

    //HELPER FUNCTIONS
    private Vector2 GetRandomLocationAroundArea()
    {
        int x = UnityEngine.Random.Range(-maxWanderRange, maxWanderRange);
        int y = UnityEngine.Random.Range(-maxWanderRange, maxWanderRange);
        return (new Vector2(moveAroundArea.x + x, moveAroundArea.y + y));
    }

    public void HasArrivedAtDestination()
    {
        isWandering = false;
    }
}
