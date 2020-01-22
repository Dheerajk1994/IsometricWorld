using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityState
{
    private string stateName;
    protected EntityState(string stateName)
    {
        this.stateName = stateName;
    }
    public virtual string GetStateName() { return stateName; }
    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual bool WillStop() { return false; }
    public virtual void Stop() { }
}
