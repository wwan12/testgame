using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public List<TaskChain> initChains;
    public List<Task> initTasks;
    public List<Task> freeTasks;
    public List<TaskChain> chainTasks;
    public Queue<Task> lockTasks;

    private TaskMenu menu;
    public float refreshTime=1f;
    private IEnumerator awakeCheck;
    // Start is called before the first frame update
    void Start()
    {
        initChains = new List<TaskChain>();
        initTasks = new List<Task>();
        freeTasks = new List<Task>();
        chainTasks = new List<TaskChain>();
        lockTasks = new Queue<Task>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Register(Task task) {
        initTasks.Add(task);
        if (awakeCheck==null)
        {
            awakeCheck = AwakeCheck();
            StartCoroutine(awakeCheck);
        }
    }

    public void Register(TaskChain taskChain)
    {
        initChains.Add(taskChain);
        if (awakeCheck == null)
        {
            awakeCheck = AwakeCheck();
            StartCoroutine(awakeCheck);
        }
    }

    public void LogOff(Task task)
    {
        if (initTasks.Contains(task))
        {
            initTasks.Remove(task);
        }
        if (freeTasks.Contains(task))
        {
            freeTasks.Remove(task);
        }
    }

    public Task FindTask(string taskName) {
        foreach (var task in freeTasks)
        {
            if (task.name.Equals(taskName))
            {
               
                return task;
            }            
        }
        return null;
    }

    public TaskChain FindChain(string taskName)
    {
        foreach (var chain in chainTasks)
        {
            if (chain.name.Equals(taskName))
            {

                return chain;
            }
        }
        return null;
    }

    public void AddWorkOrder(TaskMenu menu)
    {
        this.menu = menu;
        foreach (var task in freeTasks)
        {
            menu.SetTasks(task);
        }
        foreach (var chain in chainTasks)
        {
            menu.SetTasks(chain);
        }
        
    }

    IEnumerator AwakeCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(refreshTime);
            foreach (var task in initTasks)
            {
                if (task.taskControl.OnAwake())
                {                  
                    freeTasks.Add(task);
                    initTasks.Remove(task);
                }
            }
            foreach (var chain in initChains)
            {
                if (chain.chainControl.OnAwake())
                {              
                    chainTasks.Add(chain);
                    initChains.Remove(chain);
                }
            }
        }
       
    }

    public void ReadTask(Dictionary<string,int> tasks)
    {

    }

}
