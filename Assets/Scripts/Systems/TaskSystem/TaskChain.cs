using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "自定义任务链", menuName = "自定义生成系统/任务链")]
public class TaskChain : Task
{
    [Tooltip("当前序号")]
    public int chainIndex;
    [Tooltip("链任务，按次序添加")]
    public Task[] taskChain;

}
