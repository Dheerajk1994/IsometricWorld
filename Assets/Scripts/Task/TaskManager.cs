using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager instance;

    Queue<Task> taskQueue;

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
        taskQueue = new Queue<Task>();
    }

    public void AddTask(Task task)
    {
        taskQueue.Enqueue(task);
    }

    public Task GetTask()
    {
        if(taskQueue.Count > 0)
        {
            return taskQueue.Dequeue();
        }
        else
        {
            return null;
        }
    }

}
