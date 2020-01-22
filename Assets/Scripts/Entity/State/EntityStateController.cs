using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityTaskExecuter))]
public class EntityStateController : MonoBehaviour
{
    EntityState currentState;

    public event Action<string> StateChangeHandler;

    private void Start()
    {
        //TEST
        currentState = new ES_Wander("Wandering", this.gameObject, 10);
        currentState.Enter();
        StateChangeHandler(currentState.GetStateName());
        //currentState = new ES_Task(this.GetComponent<EntityTaskExecuter>());
    }

    private void Update()
    {
        if(currentState != null)
        {
            currentState.Execute();
        }
    }

    public void ChangeState(EntityState newState)
    {
        if(currentState != null && currentState.WillStop())
        {
            currentState.Stop();
        }
        currentState = newState;
        currentState.Enter();
    }

}
