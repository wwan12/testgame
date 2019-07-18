﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Text;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

//todo 丢弃物品，多线程优化,动态添加的格子无法管理

public class BagManage : MonoBehaviour, IBeginDragHandler, IDragHandler
{
	private Vector3 dragOffset;
    private  GameObject extUI;
    public GameObject equip;

   // public GameObject bagItem;

    public event EventHandler<ItemInfo> UseBagItemCallBack;
    public event EventHandler<ItemInfo> DiscardBagItemCallBack;
    [HideInInspector]
    public int bagCapacity = 0;//剩余容量
    protected LatticeController[] items;
    protected int[] bagItems;//背包中的数据
    public GameObject itemInfoPanel;

    public bool isAuto = false;
    public GameObject Lattice;
    public GameObject itemInBag;
    public int allCapacity = 0;//总容量
    public int lineNum = 4;
    [Tooltip("整体距上")]
    public float top = 0;
    [Tooltip("整体距左")]
    public float left = 0;
    [Tooltip("格子间上间距")]
    public float autoTop = 0;
    [Tooltip("格子间左间距")]
    public float autoLeft = 0;
    [Tooltip("格子大小")]
    public float autoSize = 0;
    [Tooltip("是否可拖动")]
    public bool isDrag;

    /// <summary>
    /// 回调方法，注册UseBagItemCallBack
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="itemInfo"></param>
    protected virtual void UseItemCallBack(object obj, ItemInfo itemInfo)
    {
        if (extUI!=null)
        {
            if (extUI.GetComponent<BagManage>().BagAddItem(itemInfo))
            {

                extUI.GetComponent<BagManage>().SaveBagData();
                ((ItemInBagController)obj).DiscardItem();
                //BagUsedItem(itemInfo.);
            }            
        }
      
        UseBagItemCallBack(obj, itemInfo);
        Messenger.Broadcast<ItemInfo>(EventCode.BAG_USE_ITEM,itemInfo);
    }


    /// <summary>
    /// 丢弃物品，不可用，注册DiscardBagItemCallBack
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="itemInfo"></param>
    private void DiscardItemCallBack(object obj, ItemInfo itemInfo) {
        DiscardBagItemCallBack(obj, itemInfo);
        bagCapacity++;
    }
    /// <summary>
    /// 删除一个物品，无视数量
    /// </summary>
    public void BagUsedItem(string name) {
        GetLatticeItem(name).DiscardItem();
    }
    /// <summary>
    /// 删除一个物品，无视数量
    /// </summary>
    public void BagUsedItem(int sel,string name)
    {
        GetLatticeItem(sel).DiscardItem();
    }
    /// <summary>
    ///修改物品数量
    /// </summary>
    public bool BagUsedItem(string name, int num)
    {
        if (num<0&&-num>GetTotalNum(name))
        {
            return false;
        }
       return GetLatticeItem(name).AddNum(num);
    }
    /// <summary>
    /// 给背包指定格子添加物品，当该格子被其他物品占用时返回false,这个方法是不安全的
    /// </summary>
    public virtual bool BagAddItem(int serialNumber, ItemInfo itemInfo)
    {
        if (serialNumber>allCapacity)
        {
            return false;
        }
        if (items[serialNumber].item==null)
        {
            GameObject item =GameObject.Instantiate(itemInBag);
            item.name = itemInfo.name;
            item.GetComponent<ItemInBagController>().info = itemInfo;
            items[serialNumber].AddItem(item);
            bagCapacity--;
            return true;
        }
        if (items[serialNumber].item.info.name.Equals(itemInfo.name))
        {
           // items[serialNumber].item.info.num+=itemInfo.num;
            return items[serialNumber].item.AddNum(itemInfo.num);
        }
        return false;
    }

    /// <summary>
    /// 给背包添加一个新的物品，提供给外部调用
    /// </summary>
    public virtual bool BagAddItem(ItemInfo itemInfo)
    {
        int index = HasItemInBag(itemInfo.name);
        if (index >= 0)
        {
            items[index].AddItem(itemInfo);
        }
        else
        {//没有找到向空格子添加一个新的
            GameObject item = GameObject.Instantiate(itemInBag);
            item.name = itemInfo.name;
            item.GetComponent<ItemInBagController>().info = itemInfo;         
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].item==null)
                {
                    items[i].AddItem(item);
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 根据名称获取物品
    /// </summary>
    /// <param name="obj"></param>
    public ItemInBagController GetLatticeItem(string name)
    {
        foreach (var item in items)
        {
            if (item.item != null && item.item.info.name.Equals(name))
            {
                return item.item;
            }
        }
        return null;
    }
    /// <summary>
    /// 根据序号获取物品
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ItemInBagController GetLatticeItem(int index)
    {
        return items[index].item==null?null: items[index].item;
    }
    /// <summary>
    /// 获取物品在背包内的总数量
    /// </summary>
    /// <param name="name"></param>
    public int GetTotalNum(string name)
    {
        int num = 0;
        foreach (var item in items)
        {
            if (item.item != null && item.item.info.name.Equals(name))
            {
                num += item.item.info.num;
            }
        }
        return num;
    }

    /// <summary>
    /// -1没有找到
    /// </summary>
    /// <param name="infoId"></param>
    /// <returns></returns>
    [Obsolete]
    public int HasItemInBag(int infoId) {
        for (int i = 0; i < bagItems.Length; i++)
        {
            if (bagItems[i]==infoId)
            {
                return i;
            }
        }
        return -1;
    }
    /// <summary>
    /// -1没有找到
    /// </summary>
    /// <param name="infoId"></param>
    /// <returns></returns>
    public int HasItemInBag(string infoName)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].item!=null)
            {
                if (items[i].item.info.name.Equals(infoName)&& items[i].item.info.num< items[i].item.info.maxNum)
                {
                    return i;
                }
            }
           
        }
        return -1;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isDrag)
        {
            Vector3 worldPos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, eventData.position, null, out worldPos))
            {
                dragOffset = new Vector3(transform.position.x - worldPos.x, transform.position.y - worldPos.y, 0f);
                transform.position = worldPos + dragOffset;
            }
        }
       
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDrag)
        {
            Vector3 worldPos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, eventData.position, null, out worldPos))
            {
                transform.position = worldPos + dragOffset;
            }
        }
      
    }

    

   
    /// <summary>
    /// 
    /// </summary>
    void NotExistItems() {
        items = new LatticeController[allCapacity];
        int existIndex = GetComponentsInChildren<LatticeController>().Length;
        if (existIndex>0)
        {           
            items.CopyTo(GetComponentsInChildren<LatticeController>(), 0); 
        }
        int j = 0;
        for (int i = existIndex; i < allCapacity; i++)
        {    
            AutoAddLattice(i,j==0?0:j/lineNum);
            j++;
        }       
        for (int i = 0; i < items.Length; i++)
         {         
            items[i].serialNumber = i;
            items[i].canvas = gameObject.GetComponent<RectTransform>();
            items[i].itemInfoPanel = itemInfoPanel;
            items[i].UseThisItemCallBack += UseItemCallBack;
         }
       
    }

    void ExistItems() {
        items = GetComponentsInChildren<LatticeController>();
        allCapacity = items.Length;           
        for (int i = 0; i < items.Length; i++)
        {
            items[i].serialNumber = i;
            items[i].canvas = gameObject.GetComponent<RectTransform>();
            items[i].itemInfoPanel = itemInfoPanel;
            items[i].UseThisItemCallBack += UseItemCallBack;
        }
       
    }
    /// <summary>
    /// 自动设置格子信息
    /// </summary>
    /// <param name="x"> 第几个</param>
    /// <param name="y">第几行</param>
     void AutoAddLattice(int x,int y)
    {
        //j加载预制体
        if (Lattice==null)
        {
           Lattice = Resources.Load<GameObject>("Prefabs/BagLattice");
        } 
        //实例化
        GameObject item = Instantiate(Lattice) as GameObject;
        item.name = x.ToString();
        RectTransform trans = item.GetComponent<RectTransform>();
    
            trans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, left+ autoLeft +(autoLeft+ autoSize) *(x%lineNum), autoSize);
            trans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,top+ autoTop+(autoTop + autoSize) * y, autoSize);
        // trans.offsetMin = Vector2.zero;
        // trans.offsetMax = Vector2.zero;
        items[x] = item.GetComponent<LatticeController>();    
        item.transform.SetParent(gameObject.GetComponent<RectTransform>().transform, false);//再将它设为canvas的子物体       
    }
    /// <summary>
    /// 0在第几格，1物品名称，2数量
    /// </summary>
    public virtual string SaveBagData()
    {
        StringBuilder saveData = new StringBuilder();
        for (int i = 0; i < items.Length; i++)
        {
            if (!items[i].IsNull())
            {
                saveData.Append(i.ToString());
                saveData.Append(",");
                saveData.Append(items[i].item.info.name.ToString());
                saveData.Append(",");
                saveData.Append(items[i].item.info.num.ToString());
                saveData.Append("|");
            }
            
        }
        return saveData.ToString();
    }
    /// <summary>
    /// 读取并恢复背包数据
    /// </summary>
    /// <param name="save"></param>
    public virtual void ReadBagData(string save) {
        string[] datas = save.Split('|');
        for (int i = 0; i < datas.Length; i++)
        {
            string[] data = datas[i].Split(',');
            BagAddItem(int.Parse(data[0]), DataToItem(data));
        }
    }

    private ItemInfo DataToItem(string[] data) {
        ItemInfo itemInfo = Warehouse.Instance.GetItemInfo(data[1]);
        itemInfo.num = int.Parse(data[2]);
        return itemInfo;
    }
    [Obsolete]
    public ItemInfo ArticlesToItem(ArticlesAttachment articles,int num)
    {
        ItemInfo itemInfo = new ItemInfo
        {
            id = articles.id,
            name = articles.name,
            note = articles.note,
            num = num,
            sprite = articles.gameObject.GetComponent<SpriteRenderer>().sprite
        };
        return itemInfo;
    }
    /// <summary>
    /// 添加其他交互页面
    /// </summary>
    /// <param name="perfabName"></param>
    public void AddOtherUI(string prefabName)
    {
        CanvasGroup group= equip.GetComponent<CanvasGroup>();
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;
        if (extUI.name.Equals(prefabName))
        {
         //   extUI.GetComponent<BagManage>().ReadBagData();
            return;
        }
        if (extUI!=null)
        {
            extUI = null;
            Destroy(extUI);
        }
       
        extUI= Resources.Load<GameObject>("prefabs/UI/" + prefabName);
        if (extUI==null)
        {
            return;
        }
        extUI = GameObject.Instantiate(extUI);
        extUI.transform.SetParent(gameObject.transform, false);
        // RectTransform trans = gameObject.GetComponent<RectTransform>();
        // trans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, , trans.sizeDelta.x);
        // trans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, top + autoTop, trans.sizeDelta.y);
        return;
    }

    /// <summary>
    /// 添加其他交互页面
    /// </summary>
    /// <param name="perfabName"></param>
    public void AddOtherUI(GameObject ui)
    {
      
        if (extUI != null)
        {
            extUI = null;
            Destroy(extUI);
        }

      //  extUI = ui;
        extUI = GameObject.Instantiate(ui);
        extUI.transform.SetParent(gameObject.transform, false);
       // return;
    }


    public void AddOtherUIData(Dictionary<int,ItemInfo> dic)
    {
        //extUI.GetComponent<BagManage>().
        foreach (var item in dic)
        {
            extUI.GetComponent<BagManage>().BagAddItem(item.Key, item.Value);
        }
        
    }


    public void ShowEquip()
    {
        CanvasGroup group = equip.GetComponent<CanvasGroup>();
        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    // Use this for initialization
    void Start()
    {
        autoSize = Screen.height / (1080 / autoSize);
        autoTop = Screen.height / (1080 / autoTop);
        autoLeft = Screen.width / (1920 / autoLeft);
        left = Screen.width / (1920 / left);
        top = Screen.height / (1080 / top);
        if (isAuto)
        {
            NotExistItems();
        }
        else
        {
            ExistItems();
        }
        bagItems = new int[allCapacity];
        bagCapacity = allCapacity;
        ShowEquip();
        Messenger.AddReturnListener<ItemInfo,bool>(EventCode.BAG_ADD_ITEM, BagAddItem);
        Messenger.AddListener<AppManage.SingleSave>(EventCode.APP_START_GAME, StartInit);
        // StartCoroutine(test());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartInit(AppManage.SingleSave save) {
        ReadBagData(save.bagData);
    }
}
