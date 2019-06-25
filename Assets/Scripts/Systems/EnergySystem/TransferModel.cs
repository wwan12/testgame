using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferModel : MonoBehaviour
{
    /// <summary>
    /// 是否供能节点
    /// </summary>
    public bool isSupply;
    /// <summary>
    /// 是否耗能节点
    /// </summary>
    public bool isUse;
    public float connectionScope = 1f;
    public List<TransferModel> energys;
    private SpriteRenderer rangeDis;
    /// <summary>
    /// 上次更新节点的事件码
    /// </summary>
    private float connectedCode;
    private float cutCode;
    // Start is called before the first frame update
    void Start()
    {
        energys = new List<TransferModel>();

        rangeDis = Resources.Load<GameObject>("prefabs/UI/RangeDisplay").GetComponent<SpriteRenderer>();
        rangeDis.size = new Vector2(connectionScope, connectionScope);
        rangeDis.transform.position = gameObject.transform.position;
        rangeDis = GameObject.Instantiate(rangeDis);
        rangeDis.transform.SetParent(gameObject.transform, false);
        rangeDis.enabled = false;

        CircleCollider2D circleCollider= gameObject.AddComponent<CircleCollider2D>();
        circleCollider.radius = connectionScope;
        circleCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 合并网格
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="uuuid"></param>
    /// <returns></returns>
    public string GridConnected(TransferModel sender,float uuuid)
    {
       
        foreach (var item in energys)
        {
            if (item.isSupply)
            {
                return item.gameObject.name;
            }
            if (!sender.Equals(item))
            {
                connectedCode = uuuid;
                if (item.connectedCode != uuuid)
                {
                    string r = item.GridConnected(this, uuuid);
                    if (r!=null)
                    {
                        return r;
                    } 
                }
            }          
        }
        return null;
    }
    /// <summary>
    /// 移除根节点，更新网格数据
    /// </summary>
    public void CutOff(TransferModel sender, float uuuid)
    {
        foreach (var item in energys)
        {
            if (item.isUse)
            {
                item.gameObject.GetComponent<ConsumptionModel>().UpDataPower();
            }
            if (!sender.Equals(item))
            {
                cutCode = uuuid;
                if (item.cutCode != uuuid)
                {
                    item.CutOff(this, uuuid);
                }
            }
        }
    }

    public float GetUUUID()
    {
        float code = Random.Range(0, 10);
        while (connectedCode==code||cutCode==code)
        {
            code = Random.Range(0, 10);
        }
       return code;
    }

    public void ShowRange()
    {
        rangeDis.enabled = true;
    }

    public void HideRange()
    {
        rangeDis.enabled = false;
    }

    public void DestroyEnergy(TransferModel e)
    {
        energys.Remove(e);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TransferModel e = collision.gameObject.GetComponent<TransferModel>();
        if (e!=null)
        {                    
            energys.Add(e);           

        }
       
      
    }
    /// <summary>
    /// todo 测试销毁是否会触发
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        TransferModel e = collision.gameObject.GetComponent<TransferModel>();
        if (e != null)
        {
            energys.Remove(e);
        }
    }
}
