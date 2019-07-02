using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 提供给科技具体实现类继承
/// </summary>
[System.Serializable]
public class TechnologyControl
{
   
    /// <summary>
    /// 开始研究时调用
    /// </summary>
    public virtual void OnStart()
    {

    }
    /// <summary>
    /// 研究完成后调用
    /// </summary>
    public virtual void OnComplete(MonoBehaviour mono)
    {

    }
}
