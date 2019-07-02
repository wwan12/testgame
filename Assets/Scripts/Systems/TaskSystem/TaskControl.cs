using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskControl
{

    public List<float> floatHeap;

    /// <summary>
    /// 触发条件
    /// </summary>
    public virtual bool OnAwake()
    {
        return false;
    }

    /// <summary>
    /// 进行中
    /// </summary>
    public virtual void OnInProgress()
    {

    }

    /// <summary>
    /// 完成后调用
    /// </summary>
    public virtual void OnComplete(MonoBehaviour mono)
    {

    }
}
