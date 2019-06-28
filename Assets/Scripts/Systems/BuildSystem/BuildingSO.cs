using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "自定义生成系统", menuName = "自定义生成系统/建筑")]
public class BuildingSO : ScriptableObject
{
    [Tooltip("建筑名称")]
    public string objectName = "Building Name";
    [Tooltip("建筑预制体")]
    public GameObject buildingPrefab;
    public Sprite lowSource;
    [Tooltip("建造时间")]
    public float buildTime;
    [Tooltip("拆除时间")]
    public float dTime=1f;
    [Tooltip("费用")]
    public Dictionary<string,int> cost;
    [Tooltip("耐久")]
    public float durable=10f;
    //public Icon icon;
    [Tooltip("特殊类型")]
    public BuildType type;
    [Tooltip("采集的资源")]
    public ResourceType res;
    [Tooltip("每个循环采集数量")]
    public int collectNum;
    [Tooltip("采集间隔")]
    public float collectInterval;
    [Tooltip("该节点的层级")]
    public int hierarchy;

    [System.Serializable]
    public struct ResourcePrefab
    {
        public ResourceType type;
        public int num;
    }
    public ResourcePrefab[] costc;
    public int costLength = 0;

    public bool showPosition;
    public enum BuildType
    {
        none,
        collect,
        storage,


    }
}
