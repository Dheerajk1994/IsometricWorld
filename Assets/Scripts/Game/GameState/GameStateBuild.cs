using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateBuild : PointerIcon, IGameEventState
{
    ConstructionObject constructionObject;

    public GameStateBuild(ConstructionObject constructionObject, Camera camera) : base(constructionObject.buildingSprite, camera) {
        this.constructionObject = constructionObject;
    }
    
    //DELEGATE FOR KNOWING WHEN THE PLAYER WANTS TO PLACE A BUILDING
    public event Action<bool> HasPlaced = delegate { }; 

    public void Enter()
    {
        Debug.Log("Entered build state");
    }

    public void Execute()
    {
        base.OnPointerMove();
        //DO SOME PLACEMENT CHECK TO SEE IF YOU CAN PLACE THE OBJECT THERE

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnLeftClick();
        }
    }

    protected override void OnLeftClick()
    {
        Vector2 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        Tuple <int, int> buildCell = TerrainManager.instance.GetTilePosGivenWorldPos(mouseWorldPos);
        if (TerrainManager.instance.CanBeBuiltOn(buildCell))
        {
            TaskManager.instance.AddTask(new BuildTask(constructionObject, "build tent", buildCell));
        }
        HasPlaced(true);
    }

    protected override void OnRightClick()
    {
        Debug.Log("right mouse clicked during build game state");
    }

    public void Exit()
    {
        base.OnExit();
    }
}
