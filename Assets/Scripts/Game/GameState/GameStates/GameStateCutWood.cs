using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateCutWood : PointerIcon, IGameEventState
{
    public GameStateCutWood(Sprite cutwoodSprite, Camera camera) : base(cutwoodSprite, camera)
    {
    }

    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Execute()
    {
        base.OnPointerMove();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnLeftClick();
        }
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnLeftClick()
    {
        Vector2 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int cutCell = TerrainManager.instance.GetTilePosGivenWorldPos(mouseWorldPos);
        if (TerrainManager.instance.GetEntityTypeOn(cutCell) == StaticEntityType.Tree)
        {
            TaskManager.instance.AddTask(new ChopTreeTask("chop tree", cutCell, 500f));
        }
    }
}
