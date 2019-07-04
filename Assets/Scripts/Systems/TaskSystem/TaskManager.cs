using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
   // public List<TaskChain> initChains;
    public List<Task> initTasks;
    public List<Task> freeTasks;
    public Queue<Task> lockTasks;

    private TaskMenu menu;
    public float refreshTime=1f;
    private IEnumerator awakeCheck;
    public Task activatedTask;

    private List<string> compeleTask;
    // Start is called before the first frame update
    void Start()
    {
       // initChains = new List<TaskChain>();
        initTasks = new List<Task>();
        freeTasks = new List<Task>();
        lockTasks = new Queue<Task>();
        compeleTask = new List<string>();

       Task[] tasks= Resources.LoadAll<Task>("");
        initTasks.AddRange(tasks);
        if (AppManage.Instance.isInGame)
        {
            ReadTask(AppManage.Instance.saveData.ongoingTasks);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 注册任务
    /// </summary>
    /// <param name="task"></param>
    public void Register(Task task) {
        
        initTasks.Add(task);
        if (awakeCheck==null)
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
            MenuSet();
        }
        compeleTask.Add(task.name);
    }


    private void MenuSet()
    {
        if (menu != null)
        {
            menu.SetTasks();
        }
    }

    /// <summary>
    /// 寻找某个任务
    /// </summary>
    /// <param name="taskName"></param>
    /// <returns></returns>
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

    public void AddWorkOrder(TaskMenu menu)
    {
        this.menu = menu;               
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
                    freeTasks.Sort((a, b) => a.type.CompareTo(b.type));
                    initTasks.Remove(task);
                    MenuSet();
                }
            }
           
        }
       
    }

    public TaskState[] SaveTask()
    {
        TaskState[] tasks = new TaskState[freeTasks.Count];
        for (int i = 0; i < freeTasks.Count; i++)
        {
            TaskState taskState = new TaskState()
            {
                name = freeTasks[i].name,
                progress = freeTasks[i].chainProgress,
                completed = freeTasks[i].taskControl.intHeap,
                total= freeTasks[i].taskControl.totalHeap,
            };
            tasks[i] = taskState;
        }
       
        
        return tasks;
    }


    public void ReadTask(TaskState[] tasks)
    {
        foreach (var item in tasks)
        {
            foreach (var init in initTasks)
            {
                if (init.name.Equals(item.name))
                {
                    init.taskControl.Recovery(item);
                    init.taskControl.OnInProgress();
                    freeTasks.Add(init);
                    initTasks.Remove(init);
                    break;
                }
            }
            
        }
    }

    [System.Serializable]
    public class TaskState
    {/// <summary>
    /// 正在进行的
    /// </summary>
        public string name;
        public int progress;
        public Dictionary<string, int> completed;
        public Dictionary<string,int> total;
      
    }
}
