using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using External;

public class Warehouse
{
    private static readonly Warehouse _instance = new Warehouse();
    /// <summary>
    /// 可以放到背包里的物品
    /// </summary>
    public  List<Item> itemTable;

    public static Warehouse Instance
    {
        get
        {
            return _instance;
        }
    }

    private Warehouse() { Init(); }

    private void Init()
    {
       
    }
 

    /// <summary>
    /// 通过名称获取仓库物品的详细信息
    /// </summary>
    /// <param name="id"></param>
    public ItemInfo GetItemInfo(string name)
    {
        foreach (var item in itemTable)
        {
            if (item.name.Equals(name))
            {
                return new ItemInfo() {
                    name = item.name,
                    id = item.id,
                    note = item.note,
                    type = item.type,
                    maxNum = item.superposition,
                    cost = item.cost,
                    sprite = Resources.Load<Sprite>("Texture/Items/"+item.icon),
                    
                };
            }
           
        }
        return null;
    }
 
   
}
