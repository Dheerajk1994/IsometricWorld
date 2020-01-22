using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DwellerTaskShow : MonoBehaviour
{
    public TextMeshPro taskText;
    void Start()
    {
        taskText.GetComponent<MeshRenderer>().sortingOrder = 5;
        this.gameObject.GetComponent<EntityTaskExecuter>().TaskChangeHandler += UpdateTaskText;
    }

    public void UpdateTaskText(string task)
    {
        taskText.GetComponent<TextMeshPro>().text = "Task: " + task;
    }
}
