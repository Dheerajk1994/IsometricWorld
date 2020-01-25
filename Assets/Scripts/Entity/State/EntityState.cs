using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityState
{
    private string stateName;

    public event Action StateDoneHandler = delegate { };

    protected EntityState(string stateName)
    {
        this.stateName = stateName;
    }
    public virtual string GetStateName() { return stateName; }
    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual bool WillStop() { return false; }
    public virtual void Stop() { }
    public virtual void OnStateDone() {
        Debug.Log("base on state done called");
        StateDoneHandler();
    }
}
