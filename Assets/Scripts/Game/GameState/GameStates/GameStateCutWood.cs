using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateCutWood : PointerIcon, IGameEventState
{
    public GameStateCutWood(Sprite cutwoodSprite, Camera camera) : base(cutwoodSprite, camera)
    {
    }

    public void StateEnter()
    {
        //throw new System.NotImplementedException();
    }

    public void StateExecute()
    {
        base.OnPointerMove();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnLeftClick();
        }
    }

    public void StateExit()
    {
        base.OnExit();
    }

    protected override void OnLeftClick()
    {
        Vector2 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int cutCell = TerrainManager.instance.GetTilePosGivenWorldPos(mouseWorldPos);
        if (TerrainManager.instance.GetEntityTypeOn(cutCell) == StaticEntityType.Tree_Pine)
        {
            TaskManager.instance.AddTask(new ChopTreeTask("chop tree", cutCell, 500f));
        }
    }
}
