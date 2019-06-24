/// <summary>
/// 事件代码
/// </summary>
public class EventCode
{
    /// <summary>
    /// 开始事件，需要初始化的组件订阅
    /// </summary>
    public static readonly string START_GAME = "START_GAME";
    /// <summary>
    /// 继续事件，当这个事件发出后，存档对象就已经准备完毕了
    /// </summary>
    public static readonly string CONTINUE_GAME = "CONTINUE_GAME";
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
    /// 添加资源,传入参数为字典《string,int》
    /// </summary>
    public static readonly string ADD_RESOURCE = "ADD_RESOURCE";
    /// <summary>
    /// 减少资源,传入参数为字典《string,int》
    /// </summary>
    public static readonly string REDUCE_RESOURCE = "REDUCE_RESOURCE";
    /// <summary>
    /// 用于注册资源是否扣除成功的回调（ _后面接 name+GetInstanceID,返回为bool）
    /// 只区分发送对象
    /// </summary>
    public static readonly string CHECK_RESOURCE = "CHECK_RESOURCE_";
    /// <summary>
    /// 播放音频，参数音频名称（不需要路径）
    /// </summary>
    public static readonly string PLAY_AUDIO = "PLAY_AUDIO";

    /// <summary>
    /// 减少地图地块资源，回调为成功减少值
    /// </summary>
    public static readonly string MAP_REDUCE_RESOURSE = "MAP_REDUCE_RESOURSE";
}
