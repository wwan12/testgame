using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "资源系统", menuName = "资源系统/新资源类型")]
public class ResourceType : ScriptableObject
{
    public string resName;
    public Sprite resSprite;
    public string count;
    public float weight;
    public Color mapColor;
}
