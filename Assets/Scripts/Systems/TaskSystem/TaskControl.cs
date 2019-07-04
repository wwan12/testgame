using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskControl
{
    /// <summary>
    /// 需要根据进度改变的堆
    /// </summary>
    public Dictionary<string,int> intHeap;
    /// <summary>
    /// 已完成的需要的堆
    /// </summary>
    public Dictionary<string,int> totalHeap;

   
    /// <summary>
    /// 注册后每个周期调用 触发条件
    /// </summary>
    public abstract bool OnAwake();

    public virtual void OnStart()
    {
        intHeap = new Dictionary<string, int>();
        totalHeap = new Dictionary<string, int>();                
    }
    /// <summary>
    /// 恢复任务状态
    /// </summary>
    /// <param name="state"></param>
    public virtual void Recovery(TaskManager.TaskState state)
    {
        intHeap = state.completed;
        totalHeap = state.total;
       
    }

    /// <summary>
    /// 被激活时调用
    /// </summary>
    public abstract void OnInProgress();




    /// <summary>
    /// 完成后实现类调用
    /// </summary>
    public void OnComplete(Task task) {
        GameObject.FindObjectOfType<TaskManager>().LogOff(task);
    }
}
