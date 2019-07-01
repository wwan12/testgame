using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "自定义生成系统", menuName = "自定义生成系统/科技")]
public class Technology : ScriptableObject
{
    [Tooltip("描述")]
   public string note;
    [Tooltip("层级")]
    public int hierarchy;
    [Tooltip("初始是否科研及")]
    public bool isResearch;
    [Tooltip("预览图")]
    public Sprite lowSource;
    [Tooltip("研究时间")]
    public float researhTime;
}
