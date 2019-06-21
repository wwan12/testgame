using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "自定义生成系统", menuName = "自定义生成系统/敌人")]
public class Enemy : ScriptableObject
{
    [Tooltip("名称")]
    public float eName;
    [Tooltip("hp")]
    public float hp;
    
    [Tooltip("预制体")]
    public GameObject perfab;

    [Tooltip("速度")]
    public float speed;
 

}
