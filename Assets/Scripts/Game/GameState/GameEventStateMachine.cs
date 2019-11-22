using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventStateMachine : MonoBehaviour
{
    IGameEventState currentState;
    IGameEventState previousState;

    public void ChangeState(IGameEventState newState)
    {
        if(currentState != null)
        {
            previousState = currentState;
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }

    private void Update()
    {
        StateUpdate();
    }

    private void StateUpdate()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }

    public void SwitchToPreviousState()
    {
        if(previousState != null)
        {
            currentState.Exit();
            currentState = previousState;
            currentState.Enter();
        }
    }
}
