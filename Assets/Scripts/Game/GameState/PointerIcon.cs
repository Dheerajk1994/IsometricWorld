using System;
using System.Collections;
using UnityEngine;

public class PointerIcon
{
    protected GameObject tile;
    protected Camera camera;

    protected PointerIcon(Sprite tileSprite, Camera camera)
    {
        this.tile = new GameObject();
        this.tile.AddComponent<SpriteRenderer>().sprite = tileSprite;
        this.tile.GetComponent<SpriteRenderer>().sortingOrder = 2;
        this.camera = camera;
    }

    protected virtual void OnPointerMove()
    {
        TerrainManager.instance.DisplayTileAtPosition(ref tile, camera.ScreenToWorldPoint(Input.mousePosition));
    }

    protected virtual void OnLeftClick() { }
    protected virtual void OnRightClick() { }

    protected void OnExit()
    {
        GameObject.Destroy(tile.gameObject);
    }
}


