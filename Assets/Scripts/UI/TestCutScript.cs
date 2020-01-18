using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCutScript : MonoBehaviour
{
    public Sprite cutwoodsprite;
    public void CutTree()
    {
        GameManager.instance.RequestNewState(new GameStateCutWood(cutwoodsprite, Camera.main));
    }
}
