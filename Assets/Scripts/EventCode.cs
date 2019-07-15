using System;
/// <summary>
/// 事件代码表
/// </summary>
public class EventCode
{
    //整体事件
    /// <summary>
    /// 开始事件，需要延后初始化的组件订阅
    /// </summary>
    public static readonly string APP_START_GAME = "APP_START_GAME";
    /// <summary>
    /// 继续事件，当这个事件发出后，存档对象就已经准备完毕了
    /// </summary>
    [Obsolete]
    public static readonly string APP_CONTINUE_GAME = "APP_CONTINUE_GAME";
    /// <summary>
    /// 存档事件，当这个事件发出后,存档开始
    /// </summary>
    public static readonly string APP_SAVE_GAME = "APP_SAVE_GAME";
    /// <summary>
    /// 存档完成事件，当这个事件发出后，存档结束
    /// </summary>
    public static readonly string APP_SAVEOVER_GAME = "APP_SAVEOVER_GAME";

    //
    /// <summary>
    /// 往背包中添加一个物体
    /// </summary>
    public static readonly string BAG_ADD_ITEM = "BAG_ADD_ITEM";
    /// <summary>
    /// 往背包使用一个物品
    /// </summary>
    public static readonly string BAG_USE_ITEM = "BAG_USE_ITEM";
    /// <summary>
    /// 开始旁白对话，
    /// </summary>
    public static readonly string DIALOGUE_START_ASIDE = "DIALOGUE_START_ASIDE";
    /// <summary>
    /// 建造这个建筑
    /// </summary>
    public static readonly string BUILD_THIS = "BUILD_THIS";
    /// <summary>
    /// 研究完成
    /// </summary>
    public static readonly string TEC_COMPLETE = "TEC_COMPLETE";

    /// <summary>
    /// 添加资源,传入参数为字典《string,int》
    /// </summary>
    public static readonly string RESOURCE_ADD = "RESOURCE_ADD";
    /// <summary>
    /// 减少资源,传入参数为字典《string,int》
    /// </summary>
    public static readonly string RESOURCE_REDUCE = "RESOURCE_REDUCE";
    /// <summary>
    /// 用于注册资源是否扣除成功的回调有返回值
    /// </summary>
    public static readonly string RESOURCE_CHECK = "RESOURCE_CHECK";
    /// <summary>
    /// 播放效果音频，参数音频名称（不需要路径）
    /// </summary>
    public static readonly string AUDIO_EFFECT_PLAY = "AUDIO_EFFECT_PLAY";
    /// <summary>
    /// 播放背景音频，参数路径，又在播放的会被覆盖
    /// </summary>
    public static readonly string AUDIO_AMBIENT_PLAY = "AUDIO_AMBIENT_PLAY";
    /// <summary>
    /// 改变背景音频音量
    /// </summary>
    public static readonly string AUDIO_AMBIENT_VOLUME = "AUDIO_AMBIENT_VOLUME";
    /// <summary>
    /// 改变效果音频音量
    /// </summary>
    public static readonly string AUDIO_EFFECT_VOLUME = "AUDIO_EFFECT_VOLUME";


    /// <summary>
    /// 减少地图地块资源，回调为成功减少值
    /// </summary>
    public static readonly string MAP_REDUCE_RESOURSE = "MAP_REDUCE_RESOURSE";
    /// <summary>
    /// 状态改变
    /// </summary>
    public static readonly string PLAYER_STATE_CHANGE = "PLAYER_STATE_CHANGE";
    /// <summary>
    /// 更新选中UI
    /// </summary>
    public static readonly string UI_SELECT_INFO = "UI_SELECT_INFO";

    /// <summary>
    /// 某个对象被消灭
    /// </summary>
    public static readonly string ENEMY_DEAD = "ENEMY_DEAD";
    /// <summary>
    /// 添加一个电力节点
    /// </summary>
    [Obsolete]
    public static readonly string ADD_POWER_NODE = "ADD_POWER_NODE";

    /// <summary>
    /// 移除一个电力节点
    /// </summary>
    [Obsolete]
    public static readonly string REMOVE_POWER_NODE = "ADD_POWER_NODE";
}
