using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// 物品抓取控制器，实现用鼠标拖动物品功能。
/// </summary>
public class ItemInBagController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerEnterHandler, IPointerExitHandler,IDropHandler, IPointerClickHandler
{
    public RectTransform canvas;
    // 储存物品最放的最近一个物品栏
    public RectTransform lastSlot;

    // 这个参数用来调整鼠标点击时，鼠标坐标与物品坐标的偏移量
    private Vector3 dragOffset;
    private CanvasGroup canvasGroup;

    public GameObject itemInfoPanel; // 物品信息面板
    public Vector2 offset; // Tooltips 面板偏移量

    private static float hoverTimer = 0f; // 鼠标悬停时间
    private static GameObject tooltips = null; // 属性面板的唯一引用

    private float timer = 0f;
    private bool pointEntered = false;

    private ItemInfo info;
    /// <summary>
    /// 使用当前物品的事件
    /// </summary>
    public event EventHandler UseItemCallBack;

    private void Start()
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

    public void OnBeginDrag(PointerEventData eventData)
    {
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
        Vector3 worldPos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas, eventData.position, null, out worldPos))
        {
            transform.position = worldPos + dragOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
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
        lastSlot.GetComponent<DropController>().HideColor();
        var dc = eventData.pointerDrag.GetComponent<ItemInBagController>();
        var tempSlot = dc.lastSlot;
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
        UseItemCallBack(this,info);
       // lastSlot.GetComponent<DropController>().HideColor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointEntered = true;
        if (eventData.dragging)
        {
            if (tooltips != null)
            {
                tooltips.SetActive(false);
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
            tooltips = Instantiate(itemInfoPanel, initPos, Quaternion.identity);
            tooltips.transform.SetParent(canvas);
        }
        tooltips.transform.position = initPos;
        tooltips.GetComponent<ItemInfoPanel>().SetInfoPanel(info);
        tooltips.SetActive(true);
    }
}
