using System;
using UnityEngine;

public enum ItemType
{
    Material,//材料
    Consumables,//消耗品
    ExtEquip,
    Armed_A_Equip,
    Armed_B_Equip,
    Exoskeleton_Equip,//外骨骼
    Reactor_Equip,//反应堆
    EnergyStorageCore_Equip,//储能电芯
    auxiliaryChip_Equip,
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
}