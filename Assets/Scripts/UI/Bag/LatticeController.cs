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
    [Obsolete("拖动物品兼容不好")]
    public bool isNull = true;
    [Tooltip("支持的item类型，默认为全支持")]
    public ItemType[] tagOfSupport;

    //public 

    void Start()
    {
        hightLightColor = new Color(1f, 1f, 1f, 0.8f);
        hideColor = new Color(1f, 1f, 1f, 0.22f);
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
        item.AddNum(itemInfo.num);
    }
    /// <summary>
    /// 添加一个新物品
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(GameObject itemController)
    {
        if (IsInTypes(itemController.GetComponent<ItemInBagController>().info.type))
        {
            isNull = false;
            item = itemController.GetComponent<ItemInBagController>();
            item.gameObject.transform.SetParent(gameObject.transform, false);
            item.UseItemCallBack += UseItemCallBack;
            item.canvas = canvas;
            item.itemInfoPanel = itemInfoPanel;
            item.offset = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, -gameObject.GetComponent<RectTransform>().sizeDelta.y);
            item.ShowItem();
          
        }

    } 

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dragItem = eventData.pointerDrag;
        if (IsInTypes(dragItem.GetComponent<ItemInBagController>().info.type))
        {
            isNull = false;          
            item= dragItem.GetComponent<ItemInBagController>();
            item.PutItem(transform as RectTransform);
            HideColor();
        }   
    }

    public bool IsInTypes(ItemType type)
    {
        if (tagOfSupport == null||tagOfSupport.Length==0)
        {
            return true;
        }
        else
        {
            foreach (var tag in tagOfSupport)
            {
                if (tag.Equals(type))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsNull() {
        if (item==null)
        {
            item = GetComponentInChildren<ItemInBagController>();
            if (item == null)
            {
                return false;
            }
        }
        else
        {
            return true;
        }
        return true;
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
