using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DwellerStatusShow : MonoBehaviour
{
    public TextMeshPro statusText;
    void Start()
    {
        statusText.GetComponent<MeshRenderer>().sortingOrder = 5;
        this.gameObject.GetComponent<EntityTaskExecuter>().TaskChangeHandler += UpdateTaskText;
    }

    public void UpdateTaskText(string task)
    {
        statusText.GetComponent<TextMeshPro>().text = task;
    }
}
