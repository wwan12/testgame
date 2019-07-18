using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "自定义剧本", menuName = "自定义生成系统/剧本/剧本")]
public class Story : ScriptableObject
{
    public string name;
    [Range(0,1)]
    public float difficulty;
    [Range(0,1)]
    public float degreeCivilization;
    [Tooltip("描述")]
    public string describe;
    [Tooltip("启动时调用")]
    public UnityAction<GameTable> init;
    [Tooltip("配置表")]
    public GameTable table;
}
