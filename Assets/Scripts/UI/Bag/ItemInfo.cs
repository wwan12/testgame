using BattleSystem;
using UnityEngine;

public enum ItemType
{
    Material,//材料
    Consumables,//消耗品
    ExtEquip,
    Armed_0_Equip,
    Armed_1_Equip,
    Exoskeleton_Equip,//外骨骼
    Reactor_Equip,//反应堆
    EnergyStorageCore_Equip,//储能电芯
    AuxiliaryChip_Equip,//辅助芯片
    Head_Equip,//头部装备
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