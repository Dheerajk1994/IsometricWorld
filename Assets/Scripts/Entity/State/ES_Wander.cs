using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ES_Wander : EntityState
{
    GameObject entity;
    EntityMovement entityMovement;
    private int maxWanderRange;
    private Vector2Int moveAroundArea;

    private readonly float timeToWaitUntilNextWanderRequest = 3f;
    private float currentWaitTime = 0f;

    private bool isWandering = false;

    //CONSTRUCTOR
    public ES_Wander(string stateName, GameObject entity, int maxWanderRange)
        :
        base(stateName)
    {
        this.entity = entity;
        this.maxWanderRange = maxWanderRange;

        entityMovement = entity.GetComponent<EntityMovement>();
        entityMovement.DestinationReachedHandler += HasArrivedAtDestination;
    }

    //INTERFACE IMPLEMENTATION

    public override void Enter()
    {
        currentWaitTime = 0f;
        isWandering = false;
        this.entity.GetComponent<EntityMovement>().moveSpeed = 0.7f;
    }

    public override void Execute()
    {
        if(!isWandering)
        {
            if (currentWaitTime >= timeToWaitUntilNextWanderRequest)
            {
                isWandering = true;
                moveAroundArea = TerrainManager.instance.GetTilePosGivenWorldPos(this.entity.transform.position);
                GetRandomLocationAroundArea();
                Debug.Log(moveAroundArea);
                entityMovement.DestinationReachedHandler += HasArrivedAtDestination;
                entityMovement.DestinationNotReachableHandler += CantGoToDestination;
                entityMovement.Move(TerrainManager.instance.RequestPath(entity.transform.position, GetRandomLocationAroundArea()));
                currentWaitTime = 0f;
            }
            else
            {
                currentWaitTime += Time.deltaTime;
            }
        }
    }

    public override void Stop()
    {
    }

    public override bool WillStop()
    {
        return true;
    }

    //HELPER FUNCTIONS
    private Vector2Int GetRandomLocationAroundArea()
    {
        int xr = UnityEngine.Random.Range(-maxWanderRange, maxWanderRange);
        int yr = UnityEngine.Random.Range(-maxWanderRange, maxWanderRange);
        return (new Vector2Int(moveAroundArea.x + xr, moveAroundArea.y + yr));
    }

    public void HasArrivedAtDestination()
    {
        entityMovement.DestinationReachedHandler -= HasArrivedAtDestination;
        entityMovement.DestinationNotReachableHandler -= CantGoToDestination;
        isWandering = false;
    }

    public void CantGoToDestination(string reason)
    {
        entityMovement.DestinationReachedHandler -= HasArrivedAtDestination;
        entityMovement.DestinationNotReachableHandler -= CantGoToDestination;
        isWandering = false;
        currentWaitTime = timeToWaitUntilNextWanderRequest;
    }
}
