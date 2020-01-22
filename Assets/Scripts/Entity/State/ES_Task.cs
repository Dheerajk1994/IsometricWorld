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
    }


    public override void Enter()
    {
        throw new System.NotImplementedException();
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
