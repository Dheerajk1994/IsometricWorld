using System;
using System.Collections;
using UnityEngine;

public class PointerIcon
{
    private GameObject tile;
    protected Camera camera;

    protected PointerIcon(Sprite tileSprite, Camera camera)
    {
        this.tile = new GameObject();
        this.tile.AddComponent<SpriteRenderer>().sprite = tileSprite;
        this.tile.GetComponent<SpriteRenderer>().sortingOrder = 2;
        this.camera = camera;
    }

    protected void OnPointerMove()
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


