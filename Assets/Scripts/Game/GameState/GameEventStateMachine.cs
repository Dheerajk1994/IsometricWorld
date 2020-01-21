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
            currentState.StateExit();
        }
        currentState = newState;
        currentState.StateEnter();
    }

    private void Update()
    {
        StateUpdate();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchToPreviousState();
        }
    }

    private void StateUpdate()
    {
        if (currentState != null)
        {
            currentState.StateExecute();
        }
    }

    public void SwitchToNormalState()
    {

    }

    public void SwitchToPreviousState()
    {
        if(previousState != null)
        {
            currentState.StateExit();
            currentState = previousState;
            currentState.StateEnter();
        }
    }
}
