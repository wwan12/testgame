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
    public event EventHandler<ItemInfo> UseThisItemCallBack;
    [HideInInspector]
    public RectTransform canvas;
    [HideInInspector]
    public GameObject itemInfoPanel; // 物品信息面板
    [HideInInspector]
    public bool isNull = true;

    //public 

    void Start()
    {
        hightLightColor = new Color(1f, 1f, 1f, 0.2f);
        hideColor = new Color(1f, 1f, 1f, 0f);
        image = GetComponent<Image>();
        item = GetComponentInChildren<ItemInBagController>();
        if (item==null)
        {
            isNull = true;
        }
        else
        {
            item.UseItemCallBack += UseItemCallBack;
            item.canvas = canvas;
            item.itemInfoPanel = itemInfoPanel;
            isNull = false;
        }
     
    }

    private void UseItemCallBack(object obj, ItemInfo itemInfo) {
        UseThisItemCallBack(obj,itemInfo);
    }
    /// <summary>
    /// 背包有这个物品添加时调用
    /// </summary>
    /// <param name="itemInfo"></param>
    public void AddItem(ItemInfo itemInfo) {
        isNull = false;
        item.AddNum(itemInfo);
    }
    /// <summary>
    /// 添加一个新物品
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(GameObject itemController)
    {
        isNull = false;
        item = itemController.GetComponent<ItemInBagController>();
        itemController.transform.SetParent(gameObject.GetComponent<RectTransform>().transform, false);
        item.UseItemCallBack += UseItemCallBack;
        item.canvas = canvas;
        item.itemInfoPanel = itemInfoPanel;
        item.offset = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x/2, gameObject.GetComponent<RectTransform>().sizeDelta.y / 2);
        item.AddItem();     
    }

    public void RemoveItem() {
        isNull = true;
        item.info = null;
        Destroy(item);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!isNull)
        {
            GameObject item = eventData.pointerDrag;
            item.GetComponent<ItemInBagController>().PutItem(transform as RectTransform);
            HideColor();
        }
      
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isNull&&eventData.dragging)
        {
            LightColor();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isNull && eventData.dragging)
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
