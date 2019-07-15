using BattleSystem;
using UnityEngine;

public enum ItemType
{
    Material,//����
    Consumables,//����Ʒ
    ExtEquip,
    Armed_0_Equip,
    Armed_1_Equip,
    Exoskeleton_Equip,//�����
    Reactor_Equip,//��Ӧ��
    EnergyStorageCore_Equip,//���ܵ�о
    AuxiliaryChip_Equip,//����оƬ
    Head_Equip,//ͷ��װ��
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