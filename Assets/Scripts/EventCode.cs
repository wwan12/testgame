using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCode
{
    /// <summary>
    /// 往背包中添加一个物体
    /// </summary>
    public static readonly string ADD_ITEM = "ADDITEM";
    /// <summary>
    /// 开始旁白对话，_后面接要说话的对象名称
    /// </summary>
    public static readonly string START_DIALOGUE = "START_DIALOGUE_";
    /// <summary>
    /// 建造这个建筑
    /// </summary>
    public static readonly string BUILD_THIS = "BUILD_THIS";
    /// <summary>
    /// 添加资源,负数为减少资源,传入参数为字典《string,int》
    /// </summary>
    public static readonly string ADD_RESOURCE = "ADD_RESOURCE";
}
