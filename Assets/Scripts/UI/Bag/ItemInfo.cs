using BattleSystem;
using UnityEngine;
/// <summary>
/// ��������
/// </summary>
public enum ItemType
{
    /// <summary>
    /// ����
    /// </summary>
    Material,
    Consumables,//����Ʒ
    ExtEquip,
    Armed_Equip,
    /// <summary>
    /// �����
    /// </summary>
    Exoskeleton_Equip,//
    /// <summary>
    /// ��Ӧ��
    /// </summary>
    Reactor_Equip,//
    /// <summary>
    /// ���ܵ�о
    /// </summary>
    EnergyStorageCore_Equip,//
    /// <summary>
    /// ����оƬ
    /// </summary>
    AuxiliaryChip_Equip,//
    /// <summary>
    /// ͷ��װ��
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