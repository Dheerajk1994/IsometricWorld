using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityState
{
    void Enter();
    void Execute();
    bool WillStop();
    void Stop();
}
