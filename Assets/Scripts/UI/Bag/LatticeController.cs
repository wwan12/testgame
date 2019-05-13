using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
/// <summary>
/// 背包格子控制器
/// </summary>
public class LatticeController : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Color hightLightColor;
    private Color hideColor;
    private Image image;
    [HideInInspector]
    public ItemInBagController item;//装在这一格的物品
    [HideInInspector]
    public int serialNumber = 0;//格子序号
    public event EventHandler UseThisItemCallBack;
    [HideInInspector]
    public RectTransform canvas;
    [HideInInspector]
    public GameObject itemInfoPanel; // 物品信息面板
    [HideInInspector]
    public bool isNull = true;

    void Start()
    {
        hightLightColor = new Color(1f, 1f, 1f, 0.2f);
        hideColor = new Color(1f, 1f, 1f, 0f);
        image = GetComponent<Image>();
        item = GetComponentInChildren<ItemInBagController>();
        if (item==null||item.info==null)
        {
            isNull = true;
        }
        else
        {
            item.UseItemCallBack += new EventHandler(UseItemCallBack);
            item.canvas = canvas;
            item.itemInfoPanel = itemInfoPanel;
            isNull = false;
        }
     
    }

    private void UseItemCallBack(object obj, EventArgs itemInfo) {
        (itemInfo as ItemInfo).bagNum = serialNumber;
        UseThisItemCallBack(obj,itemInfo);
    }

    public void AddItem(ItemInfo itemInfo) {
        isNull = false;
        item.AddItem(itemInfo);
        item.UseItemCallBack += new EventHandler(UseItemCallBack);
        item.canvas = canvas;
        item.itemInfoPanel = itemInfoPanel;
    }

    public void RemoveItem() {
        isNull = true;
        item.info = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject item = eventData.pointerDrag;
        item.GetComponent<ItemInBagController>().PutItem(transform as RectTransform);
        HideColor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            LightColor();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            HideColor();
        }
    }

    public void HideColor()
    {
        image.color = hideColor;
    }
    public void LightColor() {
        image.color = hightLightColor;
    }

}
