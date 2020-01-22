using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DwellerStateShow : MonoBehaviour
{
    public TextMeshPro stateText;
    void Start()
    {
        stateText.GetComponent<MeshRenderer>().sortingOrder = 5;
        this.gameObject.GetComponent<EntityStateController>().StateChangeHandler += UpdateStateText;
    }

    public void UpdateStateText(string task)
    {
        stateText.GetComponent<TextMeshPro>().text = "State: " +  task;
    }
}
