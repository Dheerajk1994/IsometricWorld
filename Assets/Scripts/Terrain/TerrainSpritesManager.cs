using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpritesManager : MonoBehaviour
{
    [SerializeField] private Sprite sprite_roadBase;
    [SerializeField] private Sprite sprite_roadVertical;
    [SerializeField] private Sprite sprite_roadHorizontal;
    [SerializeField] private Sprite sprite_roadDiagonal_tr;
    [SerializeField] private Sprite sprite_roadDiagonal_bl;

    //SETS ROAD SPRITES - CHECKS ADJACENT TILES TO SEE IF THERE ARE ROADS
    //THEN SETS THE ROAD SPRITE ACCORDINGLY
    public void UpdateRoadSprite(in Vector2Int roadAddedIndex, int worldWidth, int worldHeight, ref GameObject currentRoadObject, ref StaticEntity[] worldEntities, in GameObject[] worldObjects)
    {
        //RemoveAllChildrenFromObject(ref currentRoadObject);
        Debug.Log(worldEntities[roadAddedIndex.y * worldWidth + roadAddedIndex.x].StaticEntityType);
        if(worldEntities[roadAddedIndex.y * worldWidth + roadAddedIndex.x] == null)
        {
            return;
        }
        int indexToCheck = 0;
        bool noConnection = true;

        //NEED HUGE OPTIMIZATION HERE

        //up
        indexToCheck = (int)((roadAddedIndex.y + 1)* worldWidth + (roadAddedIndex.x + 1));
        if (worldEntities[indexToCheck] != null && worldEntities[indexToCheck].StaticEntityType == EntityType.Road)
        {
            noConnection = false;
            AddRoadSpriteToRoad(1, ref currentRoadObject);
            AddRoadSpriteToRoad(5, ref worldObjects[indexToCheck]);
        }

        //dtr
        indexToCheck = (int)((roadAddedIndex.y) * worldWidth + (roadAddedIndex.x + 1));
        if (worldEntities[indexToCheck] != null && worldEntities[indexToCheck].StaticEntityType == EntityType.Road)
        {
            noConnection = false;
            AddRoadSpriteToRoad(2, ref currentRoadObject);
            AddRoadSpriteToRoad(6, ref worldObjects[indexToCheck]);
        }

        //right
        indexToCheck = (int)((roadAddedIndex.y - 1) * worldWidth + (roadAddedIndex.x + 1));
        if (worldEntities[indexToCheck] != null && worldEntities[indexToCheck].StaticEntityType == EntityType.Road)
        {
            noConnection = false;
            AddRoadSpriteToRoad(3, ref currentRoadObject);
            AddRoadSpriteToRoad(7, ref worldObjects[indexToCheck]);
        }

        //dbr
        indexToCheck = (int)((roadAddedIndex.y - 1) * worldWidth + (roadAddedIndex.x));
        if (worldEntities[indexToCheck] != null && worldEntities[indexToCheck].StaticEntityType == EntityType.Road)
        {
            noConnection = false;
            AddRoadSpriteToRoad(4, ref currentRoadObject);
            AddRoadSpriteToRoad(8, ref worldObjects[indexToCheck]);
        }

        //down
        indexToCheck = (int)((roadAddedIndex.y - 1) * worldWidth + (roadAddedIndex.x - 1));
        if (worldEntities[indexToCheck] != null && worldEntities[indexToCheck].StaticEntityType == EntityType.Road)
        {
            noConnection = false;
            AddRoadSpriteToRoad(5, ref currentRoadObject);
            AddRoadSpriteToRoad(1, ref worldObjects[indexToCheck]);
        }

        //dbl
        indexToCheck = (int)((roadAddedIndex.y) * worldWidth + (roadAddedIndex.x - 1));
        if (worldEntities[indexToCheck] != null && worldEntities[indexToCheck].StaticEntityType == EntityType.Road)
        {
            noConnection = false;
            AddRoadSpriteToRoad(6, ref currentRoadObject);
            AddRoadSpriteToRoad(2, ref worldObjects[indexToCheck]);
        }

        //left
        indexToCheck = (int)((roadAddedIndex.y + 1) * worldWidth + (roadAddedIndex.x - 1));
        if (worldEntities[indexToCheck] != null && worldEntities[indexToCheck].StaticEntityType == EntityType.Road)
        {
            noConnection = false;
            AddRoadSpriteToRoad(7, ref currentRoadObject);
            AddRoadSpriteToRoad(3, ref worldObjects[indexToCheck]);
        }

        //dtl
        indexToCheck = (int)((roadAddedIndex.y + 1) * worldWidth + (roadAddedIndex.x));
        if (worldEntities[indexToCheck] != null && worldEntities[indexToCheck].StaticEntityType == EntityType.Road)
        {
            noConnection = false;
            AddRoadSpriteToRoad(8, ref currentRoadObject);
            AddRoadSpriteToRoad(4, ref worldObjects[indexToCheck]);
        }

        //base
        if (noConnection)
        {
            AddRoadSpriteToRoad(0, ref currentRoadObject);
        }

    }

    private void RemoveAllChildrenFromObject(ref GameObject currentbject)
    {
        foreach (Transform childObject in currentbject.transform)
        {
            GameObject.Destroy(childObject);
        }
    }

    private void AddRoadSpriteToRoad(int roadDir, ref GameObject roadEntity)
    {
        GameObject newSprite = new GameObject("road sprite");
        newSprite.transform.SetParent(roadEntity.transform);
        newSprite.transform.position = roadEntity.transform.position;
        SpriteRenderer sr = newSprite.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 2;    //CAUTION
        switch (roadDir)
        {
            case 0://vertical up
                sr.sprite = sprite_roadBase;
                break;
            case 1://vertical up
                sr.sprite = sprite_roadVertical;
                break;
            case 2://diagonal tr
                sr.sprite = sprite_roadDiagonal_tr;
                break;
            case 3://horizontal left
                sr.sprite = sprite_roadHorizontal;
                break;
            case 4://diagonal br
                sr.sprite = sprite_roadDiagonal_tr;
                newSprite.transform.localScale = new Vector2(1, -1);
                break;
            case 5://vertical down
                sr.sprite = sprite_roadVertical;
                newSprite.transform.localScale = new Vector2(1, -1);
                break;
            case 6://diagonal bl
                sr.sprite = sprite_roadDiagonal_bl;
                break;
            case 7://horizonal left
                sr.sprite = sprite_roadHorizontal;
                newSprite.transform.localScale = new Vector2(-1, 1);
                break;
            case 8://diagonal tl
                sr.sprite = sprite_roadDiagonal_bl;
                newSprite.transform.localScale = new Vector2(1, -1);
                break;
            default:
                break;
        }
    }
}
