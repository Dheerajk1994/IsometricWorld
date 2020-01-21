using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateNormal : PointerIcon, IGameEventState
{
    public GameStateNormal(Sprite tileHightLight, Camera mainCamera) : base(tileHightLight, mainCamera) { }

    public void StateEnter()
    {
        //Debug.Log("Entered normal game state");
    }

    public void StateExecute()
    {
        base.OnPointerMove();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnLeftClick();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            OnRightClick();
        }
    }

    protected override void OnLeftClick()
    {
        //Debug.Log("left mouse clicked during normal game state");
    }

    protected override void OnRightClick()
    {
        Debug.Log("right mouse clicked during normal game state");
    }

    public void StateExit()
    {
        base.OnExit();
    }
}
