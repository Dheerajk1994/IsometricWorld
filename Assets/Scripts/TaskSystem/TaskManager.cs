using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager instance;

    Queue<Task> taskQueue;
    Queue<EntityStateController> workerQueue;

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
        workerQueue = new Queue<EntityStateController>();
    }

    private void Update()
    {
        if(taskQueue.Count > 0)
        {
            if(workerQueue.Count > 0)
            {
                //ask if the worker can take on a new task
                //if not just remove him
                if (workerQueue.Peek().CanChangeState())
                {
                    //workerQueue.Peek().GetComponent<EntityTaskExecuter>().GiveTask(taskQueue.Dequeue());
                    workerQueue.Peek().ChangeState(new ES_Task("Working", workerQueue.Peek().GetComponent<EntityTaskExecuter>(), taskQueue.Dequeue()));
                    workerQueue.Dequeue();
                }
                else
                {
                    workerQueue.Dequeue();
                }
            }
        }
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

    public void AddWorkerToQueue(EntityStateController worker)
    {
        Debug.Log("worker added");
        workerQueue.Enqueue(worker);
    }

}
