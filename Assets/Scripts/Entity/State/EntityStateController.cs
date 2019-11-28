using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityTaskExecuter))]
public class EntityStateController : MonoBehaviour
{
    IEntityState currentState;

    private void Start()
    {
        //TEST
        //currentState = new ES_Wander(this.gameObject, 5, new Vector2(3, 3));
        currentState = new ES_Task(this.GetComponent<EntityTaskExecuter>());
    }

    private void Update()
    {
        if(currentState != null)
        {
            currentState.Execute();
        }
    }

    public void ChangeState(IEntityState newState)
    {
        if(currentState != null && currentState.WillStop())
        {
            currentState.Stop();
        }
        currentState = newState;
        currentState.Enter();
    }

}
