using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipControl : BagManage
{
  
    void Start()
    {
     
    }

    public override bool BagAddItem(int serialNumber, ItemInfo itemInfo)
    {
        // return base.BagAddItem(serialNumber, itemInfo);
        return false;
    }

    public override bool BagAddItem(ItemInfo itemInfo)
    {
        foreach (var latt in items)
        {
            if (latt.IsInTypes(itemInfo.type))
            {
                GameObject item = GameObject.Instantiate(itemInBag);
                item.name = itemInfo.name;
                item.GetComponent<ItemInBagController>().info = itemInfo;
                latt.AddItem(item);
                return true;
            }
           
        }
        return false;
       // base.BagAddItem(itemInfo);
    }

    protected override void UseItemCallBack(object obj, ItemInfo itemInfo)
    {
        //  Messenger.Broadcast<ItemInfo>(EventCode.BAG_ADD_ITEM,itemInfo);
        if (GameObject.FindGameObjectWithTag("Bag").GetComponent<BagManage>().BagAddItem(itemInfo))
        {

            BagUsedItem(itemInfo.name);
        }
      
       // base.UseItemCallBack(obj, itemInfo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    void UseEquip(object obj,ItemInfo info)
    {

    }

    //void NextLinksPaths()
    //{
    //    foreach (var connect in connects)
    //    {
    //        float startX = gameObject.transform.position.x;
    //        float startY = gameObject.transform.position.y;
    //        float endX = connect.gameObject.transform.position.x;
    //        float endY = connect.gameObject.transform.position.y;
           
    //        List<Vector3> paths = new List<Vector3>();
           
    //        paths.Add(new Vector3(startX, -Screen.height + startY, 0));//起点
    //        if (startX < endX)//在右上
    //        {
    //              paths.Add(new Vector3(startX+(startX - endX) / 2, -Screen.height + startY, 0));
    //        }
    //        else
    //        {
    //            paths.Add(new Vector3(endX + (startX - endX) / 2, -Screen.height + startY, 0));
    //        }
    //        paths.Add(new Vector3(endX, -Screen.height + endY, 0));//结束点
    //        DrawPaths(paths);
    //    }

    //}


    //void DrawPaths(List<Vector3> paths)
    //{
    //    for (int i = 0; i < paths.Count - 1; i++)
    //    {
    //        lineImage = GameObject.Instantiate(Resources.Load<GameObject>("prefabs/Node/LineImage")).GetComponent<Image>();
    //        lineImage.name = name+"Line";
    //        if (paths[i].y - paths[i + 1].y == 0)//判断画的是横线还是竖线
    //        {
    //            lineImage.transform.position = new Vector2(paths[i].x - (paths[i].x - paths[i + 1].x) / 2, paths[i].y);
    //            lineImage.rectTransform.sizeDelta = new Vector2(Mathf.Abs(paths[i].x - paths[i + 1].x) + lineWidth, lineWidth);
    //        }
    //        else if(paths[i].x - paths[i + 1].x == 0)
    //        {
    //            lineImage.transform.position = new Vector2(paths[i].x, paths[i].y - (paths[i].y - paths[i + 1].y) / 2);
    //            lineImage.rectTransform.sizeDelta = new Vector2(lineWidth, Mathf.Abs(paths[i].y - paths[i + 1].y));
    //        }
    //        else//斜线 todo 旋转后锯齿
    //        {
    //            lineImage.transform.position = new Vector2(paths[i].x - (paths[i].x - paths[i + 1].x) / 2, paths[i].y - (paths[i].y - paths[i + 1].y) / 2);
    //            lineImage.rectTransform.sizeDelta = new Vector2(lineWidth,Vector2.Distance(paths[i], paths[i+1]));//计算起点终点两点距离);
    //            float angle = Vector3.Angle(paths[i], paths[i + 1]);
    //            lineImage.transform.localRotation = Quaternion.AngleAxis(-(angle+angle), Vector3.forward);
    //        }
          
    //        lineImage.transform.SetParent(canvas, false);
    //        lineImage.transform.SetAsFirstSibling();
    //        //line= GameObject.Instantiate<GameObject>(line);
    //    }

    //}

}
