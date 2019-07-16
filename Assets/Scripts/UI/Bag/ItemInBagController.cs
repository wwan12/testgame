using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// 物品控制器
/// </summary>
public class ItemInBagController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerEnterHandler, IPointerExitHandler,IDropHandler, IPointerClickHandler
{
    [HideInInspector]
    public RectTransform canvas;
    // 储存物品最放的最近一个物品栏
    [HideInInspector]
    public RectTransform lastSlot;

    // 这个参数用来调整鼠标点击时，鼠标坐标与物品坐标的偏移量
    private Vector3 dragOffset;
    private CanvasGroup canvasGroup;
    [HideInInspector]
    public GameObject itemInfoPanel; // 物品信息面板
    [HideInInspector]
    public Vector2 offset; // Tooltips 面板偏移量

    private float hoverTimer = 0f; // 鼠标悬停时间
    private GameObject tooltips = null; // 属性面板的唯一引用

    private float timer = 0f;
    private bool pointEntered = false;
    /// <summary>
    /// 当前的物品信息，仅供读取方便，尽量不要在背包中直接操作
    /// </summary>
    [HideInInspector]
    public ItemInfo info;
    /// <summary>
    /// 使用当前物品的事件
    /// </summary>
    public event EventHandler<ItemInfo> UseItemCallBack;
    [HideInInspector]
    public Image image;
    [HideInInspector]
    public Text text;

     void Start()
    {
   
        lastSlot = transform.parent as RectTransform;
        canvasGroup = GetComponent<CanvasGroup>();
      

    }

    void Update()
    {
        if (pointEntered && timer <= hoverTimer)
        {
            timer += Time.deltaTime;
            if (timer > hoverTimer)
            {
                PopupToolTips();
            }
        }
    }
    /// <summary>
    /// 改变物品数量
    /// </summary>
    /// <param name="num"></param>
    public bool AddNum(int num)
    {
        if (info.num + num > info.maxNum)
        {
            num = num - (info.maxNum - info.num);
            info.num = info.maxNum;
            return gameObject.transform.parent.parent.GetComponent<BagManage>().BagAddItem(new ItemInfo()
            {
                sprite = info.sprite,
                id = info.id,
                name = info.name,
                type = info.type,
                note = info.note,
                num = num,
                weight = info.weight,
                cost = info.cost,
                isUse = info.isUse,
                maxNum = info.maxNum,
                equipInfo = info.equipInfo,
            });          
        }
        
        if (info.num+num<0)
        {
            
            if (-num < gameObject.transform.parent.parent.GetComponent<BagManage>().GetTotalNum(info.name))
            {
                num = num + info.num;
                Destroy(gameObject);
                return gameObject.transform.parent.parent.GetComponent<BagManage>().BagAddItem(new ItemInfo()
                {
                    sprite = info.sprite,
                    id = info.id,
                    name = info.name,
                    type = info.type,
                    note = info.note,
                    num = num,
                    weight = info.weight,
                    cost = info.cost,
                    isUse = info.isUse,
                    maxNum = info.maxNum,
                    equipInfo = info.equipInfo,
                });
            }
           
            return false;
        }
        if (info.num+num==0)
        {
            Destroy(gameObject);
            return true;
        }
        info.num += num;
        text.text = info.num.ToString();
        return true;
    }
    /// <summary>
    /// 将物品图标以及数量显示出来
    /// </summary>
    public void ShowItem()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        image.sprite = info.sprite;
        text.text = info.num.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (info == null)
        {
            return;
        }
        transform.SetParent(canvas);
        canvasGroup.blocksRaycasts = false;
        Vector3 worldPos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas, eventData.position, null, out worldPos))
        {
            dragOffset = new Vector3(transform.position.x - worldPos.x, transform.position.y - worldPos.y, 0f);
            transform.position = worldPos + dragOffset;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (info==null)
        {
            return;
        }
        Vector3 worldPos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas, eventData.position, null, out worldPos))
        {
            transform.position = worldPos + dragOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (info == null)
        {
            return;
        }
        // 没有对齐格子时，返回原来的背包格子
        if (eventData.pointerEnter == null || eventData.pointerEnter.tag != "Slot")
        {
            PutItem(lastSlot);
        }
    }

    // 将物品放入一个物品栏内
    public void PutItem(RectTransform slot)
    {
        lastSlot = slot;
        transform.SetParent(slot);
        transform.localPosition = Vector3.zero;
        canvasGroup.blocksRaycasts = true;
    }

    // 当有其他物品想要放在自己这格时，双方交换一下位置
    public void OnDrop(PointerEventData eventData)
    {
        // 先让物品栏高亮效果消失
        lastSlot.GetComponent<LatticeController>().HideColor();
        var dc = eventData.pointerDrag.GetComponent<ItemInBagController>();
        var tempSlot = dc.lastSlot;
        tempSlot.GetComponent<LatticeController>().item = lastSlot.GetComponent<LatticeController>().item;
        lastSlot.GetComponent<LatticeController>().item = null;//
        dc.PutItem(lastSlot);
        PutItem(tempSlot);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) {
           // lastSlot.GetComponent<DropController>().LightColor();
        }
        else if (eventData.button == PointerEventData.InputButton.Middle) {
            //lastSlot.GetComponent<DropController>().LightColor();
        }
        else if (eventData.button == PointerEventData.InputButton.Right) {
            UseItem();
        }   
    }


    public void UseItem() {
        if (info.isUse)
        {
            UseItemCallBack(this, info);
        }
        // lastSlot.GetComponent<DropController>().HideColor();
    }

    public void DiscardItem()
    {
        Destroy(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointEntered = true;
        if (eventData.dragging)
        {
            if (tooltips != null)
            {
              //  tooltips.SetActive(false);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointEntered = false;
        timer = 0f;
        if (tooltips != null)
        {
            tooltips.SetActive(false);
        }
    }

    public void PopupToolTips()
    {
        Vector3 initPos = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z);
        if (tooltips == null)
        {
            tooltips = GameObject.Instantiate(itemInfoPanel, initPos, Quaternion.identity);
            tooltips.transform.SetParent(canvas);
        }
        tooltips.transform.position = initPos;
        tooltips.GetComponent<ItemInfoPanel>().SetInfoPanel(info);
        tooltips.SetActive(true);
    }
}
