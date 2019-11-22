using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBuildScript : MonoBehaviour
{
    public ConstructionObject constructionObject;
    public void Build()
    {
        GameManager.instance.RequestBuildState(constructionObject, 1);
        
    }
}
