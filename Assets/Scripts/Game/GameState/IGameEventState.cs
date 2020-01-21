using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEventState
{
    void StateEnter();
    void StateExecute();
    void StateExit();
}
