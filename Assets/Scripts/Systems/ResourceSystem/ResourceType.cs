using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "资源系统", menuName = "资源系统/新资源类型")]
public class ResourceType : ScriptableObject
{

    public string resName;
    [Tooltip("扫描显示图")]//
    public Sprite resSprite;
    //public string count;
    public float weight;
    [Tooltip("在资源地图上的颜色")]
    public Color mapColor;
    [Tooltip("归属类型")]
    public AttributionType type;
    public enum AttributionType
    {
        mineral,
        organicCompound,
        gas,
    }
}
