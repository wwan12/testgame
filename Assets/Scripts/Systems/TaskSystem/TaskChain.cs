using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "自定义生成系统", menuName = "自定义生成系统/任务链")]
public class TaskChain : ScriptableObject
{
    public Task[] tasks;
    [Tooltip("主文本")]
    public string note;
    [Tooltip("触发控制器")]
    public TaskControl chainControl;
    [Tooltip("触发控制器")]
    public TaskControl chainProgress;
    public int Progress;
}
