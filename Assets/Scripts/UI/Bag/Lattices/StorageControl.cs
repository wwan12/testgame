using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageControl : BagManage
{

    public GameObject LockObj;

    // Start is called before the first frame update
    void Start()
    {
       // lineNum=
        if (LockObj.layer == AppManage.Instance.BuildLayer)
        {
            if (AppManage.Instance.saveData.otherData.ContainsKey("build_storage_" + LockObj.transform.position.x + "|" + LockObj.transform.position.y))
            {
                ReadBagData("");
            }            
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void AutoXY(float x, float y)
    {
        //  base.AutoXY(x, y);
        GetComponent<RectTransform>().sizeDelta =new Vector2(x,y);
    }

    public override bool BagAddItem(int serialNumber, ItemInfo itemInfo)
    {
         return base.BagAddItem(serialNumber, itemInfo);
        //return false;
    }

    public override bool BagAddItem(ItemInfo itemInfo)
    {
        return base.BagAddItem(itemInfo);
    }

    protected override void UseItemCallBack(object obj, ItemInfo itemInfo)
    {
        if (GameObject.FindGameObjectWithTag("Bag").GetComponent<BagManage>().BagAddItem(itemInfo))
        {
            ((ItemInBagController)obj).DiscardItem();
            SaveBagData();
          //  BagUsedItem(itemInfo.name);
        }
        //base.UseItemCallBack(obj, itemInfo);
    }

    public override string SaveBagData()
    {
        if (LockObj.layer == AppManage.Instance.BuildLayer)
        {
            AppManage.Instance.saveData.otherData.Add("build_storage_" + LockObj.transform.position.x + "|" + LockObj.transform.position.y, base.SaveBagData());
        }
        return "";
    }

    public override void ReadBagData(string save)
    {
        if (LockObj.layer == AppManage.Instance.BuildLayer)
        {
            base.ReadBagData(AppManage.Instance.saveData.otherData["build_storage_" + LockObj.transform.position.x + "|" + LockObj.transform.position.y]);
        }
      
    }

}
