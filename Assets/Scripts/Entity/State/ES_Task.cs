using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ES_Task : EntityState
{
    EntityTaskExecuter entityTaskExecuter;

    public ES_Task(string stateName, EntityTaskExecuter entityTaskExecuter)
        :
        base(stateName)
    {
        this.entityTaskExecuter = entityTaskExecuter;
        this.entityTaskExecuter.TaskDoneHandler += OnStateDone;
    }

    public ES_Task(string stateName, EntityTaskExecuter entityTaskExecuter, Task task)
        :
        base(stateName)
    {
        this.entityTaskExecuter = entityTaskExecuter;
        this.entityTaskExecuter.GiveTask(task);
    }

    public override void Enter()
    {
        //this..GetComponent<EntityMovement>().moveSpeed = 1.5f;
    }

    public override void Execute()
    {
        if(entityTaskExecuter != null)
        {
            entityTaskExecuter.Execute();
        }
    }

    public override bool WillStop()
    {
        return true;
    }
}
