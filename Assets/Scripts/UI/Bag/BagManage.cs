using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Text;

//todo 丢弃物品，根据物品id自动寻找添加物品，多线程优化

public class BagManage : MonoBehaviour, IBeginDragHandler, IDragHandler
{
	private Vector3 dragOffset;

   // public GameObject bagItem;

    public event EventHandler UseBagItemCallBack;
    public event EventHandler DiscardBagItemCallBack;
    [HideInInspector]
    public int bagCapacity = 0;//剩余容量
    private LatticeController[] items;
    private int[] bagItems;//背包中的数据
    public GameObject itemInfoPanel;

    public bool isAuto = false;
    public GameObject Lattice;
    public int allCapacity = 0;//总容量
    public int lineNum = 4;
    public int devier = 0;
    public int autoTop = 0;
    public int autoLeft = 0;

    /// <summary>
    /// 使用某一个物品
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="itemInfo"></param>
    public void UseItem(object obj, EventArgs itemInfo)
    {
        UseBagItemCallBack(obj, itemInfo);
    }
    /// <summary>
    /// 丢弃某一个物品
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="itemInfo"></param>
    private void DiscardItemCallBack(object obj, EventArgs itemInfo) {
        DiscardBagItemCallBack(obj, itemInfo);
        bagCapacity++;
    }
    /// <summary>
    /// 使用完某一个物品，提供给外部调用
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="itemInfo"></param>
    public void BagUsedItem(object obj, EventArgs itemInfo)
    {
        (itemInfo as ItemInfo).num--;
    }
    /// <summary>
    /// 给背包添加一个新的物品，提供给外部调用
    /// </summary>
    public void BagAddItem(ItemInfo itemInfo) {
        int index = HasItemInBag(itemInfo.id);
        if (index >= 0)
        {
            items[index].AddItem(itemInfo);
        }
        else
        {//没有找到向空格子添加一个新的
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].isNull)
                {
                    items[i].AddItem(itemInfo);
                }
            }
            bagCapacity--;
        }
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
        if (items[serialNumber].isNull)
        {
            items[serialNumber].AddItem(itemInfo);
            bagCapacity--;
            return true;
        }
        if (items[serialNumber].item.info.id==itemInfo.id)
        {
            items[serialNumber].item.info.num+=itemInfo.num;
            return true;
        }
        return false;
    }
    /// <summary>
    /// -1没有找到
    /// </summary>
    /// <param name="infoId"></param>
    /// <returns></returns>
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
        CanvasGroup group= gameObject.GetComponent<CanvasGroup>();
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;
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
    }

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
            items[i].UseThisItemCallBack += new EventHandler(UseItem);
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
            items[i].UseThisItemCallBack += new EventHandler(UseItem);
        }
       
    }

     void AutoAddLattice(int x,int y)
    {
        //j加载预制体
       // UnityEngine.Object prefab = Resources.Load("Prefabs/BagLattice");
        //实例化
        GameObject item = Instantiate(Lattice) as GameObject;
        item.name = x.ToString();
        //恢复四锚点
        RectTransform trans = item.GetComponent<RectTransform>();
      
        if (x==0)
        {
            trans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, autoLeft, 32);
            trans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, autoTop, 32);
        }
        else
        {
            trans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, autoLeft+(autoLeft+trans.rect.size.x)*(x%lineNum), 32);
            trans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, autoTop+(autoTop + trans.rect.size.y) * y, 32);
        }
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
            if (!items[i].isNull)
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
            ItemInfo itemInfo = new ItemInfo
            {
                id = articles.id,
                name = articles.name,
                note = articles.note,
                num = int.Parse(data[2]),
                sprite = articles.gameObject.GetComponent<SpriteRenderer>().sprite
            };
            BagAddItem(int.Parse(data[0]), itemInfo);
        }
    }

   // Update is called once per frame
   void Update()
    {

    }
}
