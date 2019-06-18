using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂载在能够提供资源的预制体上
/// </summary>
public class ResourceComponent : MonoBehaviour
{
    private Dictionary<string, int> d_Resources;

    [System.Serializable]
    public struct ResourcePrefab
    {
        public ResourceType type;
        public int num;
    }

    public ResourcePrefab[] resources;

    // Start is called before the first frame update
    void Start()
    {
        d_Resources = new Dictionary<string, int>();
        foreach (var item in resources)
        {
            d_Resources.Add(item.type.resName, item.num);
        }
        resources = null;
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 获取该预制体资源
    /// </summary>
    public void Get()
    {
        Get(1);
        
    }
    /// <summary>
    /// 按百分比获取该预制体资源
    /// </summary>
    /// <param name="percentage">0~1</param>
    public void Get(int percentage) {
        Dictionary<string, int> g_Resources=new Dictionary<string, int>();
        foreach (var res in d_Resources)
        {
            int get = res.Value * percentage;
            d_Resources[res.Key] -= get;
            g_Resources.Add(res.Key, get);
        }
        Messenger.Broadcast<Dictionary<string, int>>(EventCode.ADD_RESOURCE, g_Resources);
    }

    /// <summary>
    /// 添加某项资源
    /// </summary>
    /// <param name="data"></param>
    public void Add(Dictionary<string, int> data)
    {
        foreach (var d in data)
        {
            if (d_Resources.ContainsKey(d.Key))
            {
                d_Resources[d.Key] += d.Value;
            }
            else
            {
                d_Resources.Add(d.Key, d.Value);
            }
        }

    }
}
