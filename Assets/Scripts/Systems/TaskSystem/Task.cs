using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "自定义生成系统", menuName = "自定义生成系统/任务")]
public class Task : ScriptableObject
{
   // [Tooltip("名称")]
   // public string name;
    [Tooltip("说明")]
    public string note;
    [Tooltip("任务序号")]
    public int TaskIndex;
    [Tooltip("任务控制器")]
    public TaskControl taskControl;
 
}


