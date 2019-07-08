using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "自定义资源类型", menuName = "自定义生成系统/新资源类型")]
public class ResourceType : ScriptableObject
{

    public string resName;
    [Tooltip("扫描显示图")]//
    public Sprite resSprite;
    //public string count;
    public float weight;
    [Tooltip("在资源地图上的颜色")]
    public Sprite mapColor;
    [Tooltip("归属类型")]
    public AttributionType type;
    public enum AttributionType
    {
        mineral,
        organicCompound,
        gas,
    }
}
