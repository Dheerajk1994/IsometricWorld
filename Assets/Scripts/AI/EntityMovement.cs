using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirections
{
    North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest
}

public class EntityMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.5f;
    private List<Vector2> path;
    private MoveDirections moveDirection;

    private bool isMoving = false;
    private int currentPathIndex = 0;
    private Vector2 nextMoveToLocation;

    public event Action<MoveDirections> MoveDirectionChangHandler = delegate { };
    public event Action DestinationReachedHandler = delegate { };

    private void Start()
    {
    }

    private void Update()
    {
        TryMove();
    }

    public void Move(List<Vector2> path) 
    {
        this.path = path;
        currentPathIndex = 0;
        if(path != null && path.Count > 0)
        {
            nextMoveToLocation = path[currentPathIndex];
            CalculateMoveDirection();
            isMoving = true;
        }
        else
        {
            isMoving = false;
            DestinationReachedHandler();
        }
        
    }

    private void TryMove()
    {
        if (isMoving)
        {
            if((Vector2)this.transform.position != nextMoveToLocation)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, nextMoveToLocation, moveSpeed * Time.deltaTime);
            }
            else
            {
                currentPathIndex++;
                if(currentPathIndex < path.Count)
                {
                    nextMoveToLocation = path[currentPathIndex];
                    CalculateMoveDirection();
                }
                else
                {
                    isMoving = false;
                    DestinationReachedHandler();
                }
            }
        }
    }

    private void CalculateMoveDirection()
    {
        Vector2 dif = (nextMoveToLocation - (Vector2)this.transform.position);
        float directionAngle = (float)(Mathf.Atan2(dif.x, dif.y)) * Mathf.Rad2Deg;
        //Debug.Log((Vector2)this.transform.position + " : " + nextMoveToLocation + " : " + directionAngle);

        //NORTH
        if (directionAngle == 0f)
        {
            moveDirection = MoveDirections.North;
        }
        //NORTHEAST
        else if (directionAngle > 60f && directionAngle < 65f)
        {
            moveDirection = MoveDirections.NorthEast;
        }
        //EAST
        else if (directionAngle == 90f)
        {
            moveDirection = MoveDirections.East;
        }
        //SOUTH EAST
        else if (directionAngle > 110f && directionAngle < 120f)
        {
            moveDirection = MoveDirections.SouthEast;
        }
        //SOUTH 
        else if (directionAngle == 180f)
        {
            moveDirection = MoveDirections.South;
        }
        //SOUTH WEST
        else if (directionAngle < -110f && directionAngle > -120f)
        {
            moveDirection = MoveDirections.SouthWest;
        }
        //WEST
        else if (directionAngle == -90f)
        {
            moveDirection = MoveDirections.West;
        }
        //NORTH WEST
        else if (directionAngle < -60f && directionAngle > -65f)
        {
            moveDirection = MoveDirections.NorthWest;
        }

        MoveDirectionChangHandler(moveDirection);
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    if(path != null)
    //    {
    //        foreach (Vector2 loc in path)
    //        {
    //            Gizmos.DrawSphere(loc, 0.2f);
    //        }
    //    }
    //}
}
