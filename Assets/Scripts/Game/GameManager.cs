using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameEventStateMachine))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private Sprite tileHighLightSprite;
    private GameEventStateMachine gameEventStateMachine;
    private Camera mainCamera;

    GameStateNormal normalState;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(this != instance)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        gameEventStateMachine = GetComponent<GameEventStateMachine>();
        mainCamera = Camera.main;
        normalState = new GameStateNormal(tileHighLightSprite, mainCamera);
        gameEventStateMachine.ChangeState(normalState);
    }


    public void RequestBuildState(ConstructionObject constructionObject, int buildingID)
    {
        GameStateBuild buildState= new GameStateBuild(constructionObject, mainCamera);
        buildState.HasPlaced += BuildingPlaced;
        gameEventStateMachine.ChangeState(buildState);
    }

    public void BuildingPlaced(bool val)
    {
        Debug.Log("building has been placed");
        normalState = new GameStateNormal(tileHighLightSprite, mainCamera);
        gameEventStateMachine.ChangeState(normalState);
    }
}
