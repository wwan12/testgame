using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Text;
using UnityEngine.SceneManagement;
using System.Collections;

//todo 丢弃物品，多线程优化,动态添加的格子无法管理

public class BagManage : MonoBehaviour, IBeginDragHandler, IDragHandler
{
	private Vector3 dragOffset;
    private GameObject extUI;

   // public GameObject bagItem;

    public event EventHandler<ItemInfo> UseBagItemCallBack;
    public event EventHandler<ItemInfo> DiscardBagItemCallBack;
    [HideInInspector]
    public int bagCapacity = 0;//剩余容量
    private LatticeController[] items;
    private int[] bagItems;//背包中的数据
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

    /// <summary>
    /// 使用某一个物品
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="itemInfo"></param>
    public void UseItem(object obj, ItemInfo itemInfo)
    {
        UseBagItemCallBack(obj, itemInfo);
    }
    /// <summary>
    /// 丢弃某一个物品
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="itemInfo"></param>
    private void DiscardItemCallBack(object obj, ItemInfo itemInfo) {
        DiscardBagItemCallBack(obj, itemInfo);
        bagCapacity++;
    }
    /// <summary>
    /// 使用完某一个物品，提供给外部调用
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="itemInfo"></param>
    [Obsolete]
    public void BagUsedItem(object obj, ItemInfo itemInfo)
    {
        (itemInfo as ItemInfo).num--;
    }
    /// <summary>
    /// 给背包指定格子添加物品，当该格子被其他物品占用时返回false
    /// </summary>
    public bool BagAddItem(int serialNumber, ItemInfo itemInfo)
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
            items[serialNumber].item.info.num+=itemInfo.num;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 给背包添加一个新的物品，提供给外部调用
    /// </summary>
    public void BagAddItem(ItemInfo itemInfo)
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
                }
            }
        }
    }

    /// <summary>
    /// 根据序号获取物品
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ItemInfo GetLatticeItem(int index)
    {
        return items[index].item==null?null: items[index].item.info;
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
            if (items[i].item.info!=null)
            {
                if (items[i].item.info.name.Equals(infoName) )
                {
                    return i;
                }
            }
           
        }
        return -1;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 worldPos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, eventData.position, null, out worldPos))
        {
            dragOffset = new Vector3(transform.position.x - worldPos.x, transform.position.y - worldPos.y, 0f);
            transform.position = worldPos + dragOffset;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, eventData.position, null, out worldPos))
        {
            transform.position = worldPos + dragOffset;
        }
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
        //AddOtherUI("PlayerEquip");
       // StartCoroutine(test());
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(2);
        BagAddItem(0,new ItemInfo()
        {
            name = "aaa" + 0,
            sprite = Resources.Load<Sprite>("Palette/2.5dmap_03_111.asset")
        });
        BagAddItem(1,new ItemInfo()
        {
            name = "aaa" + 1,
            sprite = Resources.Load<Sprite>("Palette/2.5dmap_03_111.asset")
        });
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
            items[i].UseThisItemCallBack += UseItem;
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
            items[i].UseThisItemCallBack += UseItem;
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
    public string SaveBagData()
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

    public void ReadBagData(string save) {
        string[] datas = save.Split('|');
        for (int i = 0; i < datas.Length; i++)
        {
            string[] data = datas[i].Split(',');
            ArticlesAttachment articles= Warehouse.Instance.GetAtriclesInfo(data[1]);          
            BagAddItem(int.Parse(data[0]), ArticlesToItem(articles, int.Parse(data[2])));
        }
    }

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
    public GameObject AddOtherUI(string prefabName)
    {
        if (extUI!=null)
        {
            extUI = null;
            Destroy(extUI);
        }
        extUI= Resources.Load<GameObject>("prefab/"+prefabName);
        extUI = GameObject.Instantiate(gameObject);
        extUI.transform.SetParent(gameObject.GetComponent<RectTransform>().transform, false);
        // RectTransform trans = gameObject.GetComponent<RectTransform>();
        // trans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, , trans.sizeDelta.x);
        // trans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, top + autoTop, trans.sizeDelta.y);
        return extUI;
    }

   // Update is called once per frame
    void Update()
    {

    }
}
