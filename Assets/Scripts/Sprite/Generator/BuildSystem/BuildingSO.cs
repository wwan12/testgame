using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "建筑系统", menuName = "建筑系统/建筑")]
public class BuildingSO : ScriptableObject
{
    [Tooltip("建筑名称")]
    public string objectName = "Building Name";
    [Tooltip("建筑预制体")]
    public GameObject buildingPrefab;
    [Tooltip("建造时间")]
    public float buildTime;
    [Tooltip("拆除时间")]
    public float dTime=1f;
    [Tooltip("费用")]
    public float cost;
    [Tooltip("耐久")]
    public float durable=10f;
    //public Icon icon;
}
