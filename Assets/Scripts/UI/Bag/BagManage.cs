using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class BagManage : MonoBehaviour, IBeginDragHandler, IDragHandler
{
	private Vector3 dragOffset;

    public GameObject bagItem;

    public event EventHandler UseBagItemCallBack;
    public event EventHandler DiscardBagItemCallBack;

    private int bagCapacity = 0;//剩余容量

    private LatticeController[] items;
    public GameObject itemInfoPanel;

    /// <summary>
    /// 使用某一个物品
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="itemInfo"></param>
    private void UseItemCallBack(object obj, EventArgs itemInfo)
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
    public void BagAddItem(object obj, EventArgs itemInfo) {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].isNull)
            {
                items[i].AddItem(obj, itemInfo);
            }
        }
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
        items = GetComponentsInChildren<LatticeController>();
        if (items.Length==0)
        {

        }
        else
        {
            ExistItems();
        }
    }

    void ExistItems() {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].serialNumber = i;
            items[i].canvas = gameObject.GetComponent<RectTransform>();
            items[i].itemInfoPanel = itemInfoPanel;
            items[i].UseThisItemCallBack += new EventHandler(UseItemCallBack);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
