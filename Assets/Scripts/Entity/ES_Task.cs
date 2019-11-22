using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ES_Task : IEntityState
{
    EntityTaskExecuter entityTaskExecuter;

    public ES_Task(EntityTaskExecuter entityTaskExecuter)
    {
        this.entityTaskExecuter = entityTaskExecuter;
    }

    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Execute()
    {
        if(entityTaskExecuter != null)
        {
            entityTaskExecuter.Execute();
        }
    }

    public void Stop()
    {
        throw new System.NotImplementedException();
    }

    public bool WillStop()
    {
        return true;
    }
}
