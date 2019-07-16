using BattleSystem;
using UnityEngine;
/// <summary>
/// 格子类型
/// </summary>
public enum ItemType
{
    /// <summary>
    /// 材料
    /// </summary>
    Material,
    Consumables,//消耗品
    ExtEquip,
    Armed_Equip,
    /// <summary>
    /// 外骨骼
    /// </summary>
    Exoskeleton_Equip,//
    /// <summary>
    /// 反应堆
    /// </summary>
    Reactor_Equip,//
    /// <summary>
    /// 储能电芯
    /// </summary>
    EnergyStorageCore_Equip,//
    /// <summary>
    /// 辅助芯片
    /// </summary>
    AuxiliaryChip_Equip,//
    /// <summary>
    /// 头部装备
    /// </summary>
    Head_Equip,//
    Leg_Equip,
    Body_Equip,
}

public class ItemInfo
{
    public string id;
    public string name="";
    public ItemType type;
    public string note="";
    public int num=0;
    public float weight=0;
    public float cost;
    public bool isUse=true;
    public Sprite sprite;
    public int maxNum;
    public Equip equipInfo; 
}