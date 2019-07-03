using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskControl
{
    /// <summary>
    /// 需要根据进度改变的堆
    /// </summary>
    public List<int> intHeap;
    /// <summary>
    /// 不需要改变的堆，已完成的堆
    /// </summary>
    public List<int> CompHeap;

   
    /// <summary>
    /// 注册后每个周期调用 触发条件
    /// </summary>
    public abstract bool OnAwake();

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
