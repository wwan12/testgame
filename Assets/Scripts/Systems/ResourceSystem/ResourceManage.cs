using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 资源管理
/// </summary>
public class ResourceManage : MonoBehaviour
{
    [Tooltip("载入的资源类型")]   
    public ResourceType[] resourceTypes;//载入的资源类型
    [Tooltip("要显示的canvas")]
    public RectTransform canvas;
    private Dictionary<string,int> warehouse; 
    // Start is called before the first frame update
    void Start()
    {
        warehouse = new Dictionary<string, int>();
        //foreach (var type in resourceTypes)
        //{
        //    warehouse.Add(type.resName,0);
        //}
        //if (canvas==null)
        //{
        //    canvas = GameObject.FindObjectOfType<RectTransform>();
        //}
        Messenger.AddListener<Dictionary<string, int>>(EventCode.ADD_RESOURCE, Add);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 添加某项资源
    /// </summary>
    /// <param name="data"></param>
    public void Add(Dictionary<string,int> data) {
        foreach (var d in data)
        {
            if (warehouse.ContainsKey(d.Key))
            {
                warehouse[d.Key] += d.Value;
            }
            else
            {
                warehouse.Add(d.Key, d.Value);
            }
        }
        if (canvas!=null)
        {
            ChangeCanvasInfo();
        }
       
    }
    /// <summary>
    /// 检测是否有足够的资源
    /// </summary>
    /// <param name="check"></param>
    /// <returns></returns>
    public bool Check(Dictionary<string, int> check)
    {
        foreach (var c in check)
        {
            if (warehouse.ContainsKey(c.Key))
            {
                if (warehouse[c.Key]<c.Value)
                {
                    return false;
                }              
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 忽略不存在的资源,删除前应先检查是否有足够的资源并在主线程调用
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public void Remove(Dictionary<string, int> data)
    {
        foreach (var d in data)
        {
            if (warehouse.ContainsKey(d.Key))
            {
                warehouse[d.Key] -= d.Value;
            }
        }
  
    }
    /// <summary>
    /// 查询现在资源数
    /// </summary>
    public Dictionary<string, int> Query()
    {
        return warehouse;
    }

    public ResourceType[] GetAllResourceInfo()
    {
        return resourceTypes;
    }

    public ResourceType GetResourceInfo(string name)
    {
        foreach (var type in resourceTypes)
        {
            if (name.Equals(type.resName))
            {
                return type;
            }
        }
        return null;
    }

    void ChangeCanvasInfo()
    {
        foreach (var item in warehouse)
        {
            
        }
    }
}
