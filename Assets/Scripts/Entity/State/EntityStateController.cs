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
        //currentState = new ES_Wander("Wandering", this.gameObject, 10);
        currentState = new ES_Task("Working", this.GetComponent<EntityTaskExecuter>());
        currentState.StateDoneHandler += StateDone;
        currentState.Enter();
        StateChangeHandler(currentState.GetStateName());
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
        currentState.StateDoneHandler += StateDone;
        currentState.Enter();
    }
    
    public bool CanChangeState()
    {
        return currentState.WillStop();
    }

    public void StateDone()
    {
        Debug.Log("entity state controoler state done called");
        ChangeState(new ES_Wander("Wandering", this.gameObject, 5));
    }
}
