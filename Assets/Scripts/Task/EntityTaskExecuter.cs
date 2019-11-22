using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTaskExecuter : MonoBehaviour
{
    private Task currentTask;
    private bool hasTask;
    [SerializeField] private uint workAmount = 5;

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
            currentTask.AssignTaskToEntity(this.gameObject);
            ExecuteTask();
        }
    }
    public void ExecuteTask()
    {
        if (currentTask != null)
        {
            currentTask.Execute(workAmount);
        }
    }
    public void FinishTask()
    {
        hasTask = false;
        currentTask = null;
    }
}
