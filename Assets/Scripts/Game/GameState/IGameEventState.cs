using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEventState
{
    void Enter();
    void Execute();
    void Exit();
}
