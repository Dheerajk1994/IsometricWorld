using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTaskExecuter : MonoBehaviour
{
    private Task currentTask;
    private bool hasTask;
    [SerializeField] private uint workAmount = 5;

    //Delegate
    public event Action<string> TaskChangeHandler = delegate { };

    public void Execute()
    {
        //FUTURE UPDATES - USE A DELEGATE AND PLUG
        //THE ENTITY INTO THE TASK MANAGER 
        //SO THE TASK MANAGER CAN CALL THE ENTITY WHEN IT 
        //HAS A TASK THAT NEEDS TO BE DONE
        if (!hasTask)
        {
            RequestTask();
        }
        else
        {
            ExecuteTask();
        }
    }


    //HELPERS 
    public void RequestTask()
    {
        currentTask = TaskManager.instance.GetTask();
        if (currentTask != null)
        {
            hasTask = true;
            currentTask.TaskCompleted += FinishTask;
            currentTask.TaskFailed += TaskFailed;
            currentTask.AssignTaskToEntity(this.gameObject);
            TaskChangeHandler(currentTask.TaskName);
            ExecuteTask();
        }
    }
    public void ExecuteTask()
    {
        if (currentTask != null)
        {
            currentTask.Execute(ref workAmount);
        }
    }
    public void FinishTask()
    {
        Debug.Log("Task completed: " + currentTask.TaskName);
        hasTask = false;
        currentTask = null;
        TaskChangeHandler("Idle");
    }
    public void TaskFailed(string reason)
    {
        Debug.Log("Task failed: " + currentTask.TaskName + " : " + reason);
        hasTask = false;
        currentTask = null;
    }
}
