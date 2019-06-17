using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManage : MonoBehaviour
{

    public ResourceType[] resourceTypes;
    private Dictionary<string,int> warehouse; 
    // Start is called before the first frame update
    void Start()
    {
        warehouse = new Dictionary<string, int>();
        foreach (var type in resourceTypes)
        {
            warehouse.Add(type.resName,0);
        }
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
    /// 忽略不存在的资源
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool Remove(Dictionary<string, int> data)
    {
        foreach (var d in data)
        {
            if (warehouse.ContainsKey(d.Key))
            {
                warehouse[d.Key] -= d.Value;
            }
        }
        return true;
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

    public class ResourceInfo
    {

    }
}
