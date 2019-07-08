using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "自定义派系", menuName = "自定义生成系统/派系")]
public class Factions : ScriptableObject
{
    [Tooltip("")]
    public string factName;
    /// <summary>
    /// 每400一个阶段
    /// </summary>
    [Range(-1000,1000)]
    public int initRelations;
    [Tooltip("被发现是坏东西时惩罚系数")]
    public float punish;

}
