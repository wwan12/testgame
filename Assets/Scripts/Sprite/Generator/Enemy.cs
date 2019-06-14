using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "生成系统", menuName = "生成系统/敌人")]
public class Enemy : ScriptableObject
{
    [Tooltip("hp")]
    public float hp;
    [Tooltip("AI")]
    public Ai ai;
    [Tooltip("预制体")]
    public GameObject perfab;
  
}
