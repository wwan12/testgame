using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologyManager : MonoBehaviour
{
    [Tooltip("是否从磁盘读取")]
    public bool isOSRead = true;

    private readonly string OS_PATH = "Assets/TechnologyAssets";
    [Tooltip("UI节点s")]
    public Technology[] allTechnology;
    [HideInInspector]
    public TechnologyMenu menu;
    // Start is called before the first frame update
    void Start()
    {
        if (isOSRead)
        {
            allTechnology = Resources.LoadAll<Technology>(OS_PATH);
            if (AppManage.Instance.isInGame)
            {
                ReadNodes(AppManage.Instance.saveData.tecStates);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 设置可用
    /// </summary>
    /// <param name="TechnologyName"></param>
    public void SetAvailable(string TechnologyName)
    {
        foreach (var tec in allTechnology)
        {
        
            if (tec.name.Equals(TechnologyName))
            {
                tec.isResearch = true ;
                if (menu!=null)
                {
                    menu.SetAvailable(tec.name);
                }
            }

        }
    }
    /// <summary>
    /// 检测是否研究
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool CheckComplete(string name)
    {
        foreach (var tec in allTechnology)
        {
            if (tec.name.Equals(name))
            {
                return tec.isComplete;
            }

        }
        return false;
    }
    /// <summary>
    /// 储存节点允许状态 
    /// </summary>
    /// <returns></returns>
    public TecState[] SaveNodes()
    {
        TecState[] nodes = new TecState[allTechnology.Length];

        for (int i = 0; i < nodes.Length; i++)
        {
            TecState tec = new TecState
            {
                name = allTechnology[i].name,
                isResearch = allTechnology[i].isResearch,
                isComplete = allTechnology[i].isComplete,
                hierarchy = i,
                progress = allTechnology[i].progress
            };           
            nodes[i]=tec;
        }

        return nodes;
    }

    /// <summary>
    /// 恢复节点允许状态 
    /// </summary>
    /// <param name="pairs"></param>
    public void ReadNodes(TecState[] pairs)
    {
        if (pairs == null)
        {
            return;
        }
        foreach (var pair in pairs)
        {
            foreach (var tec in allTechnology)
            {
                if (tec.name.Equals(pair.name))
                {                   
                    tec.isComplete = pair.isComplete;
                    tec.isResearch = pair.isResearch;
                    tec.progress = pair.progress;

                }
            }
           
        }

    }

    [System.Serializable]
    public class TecState
    {
        public int hierarchy;
        public string name;
        public bool isResearch;
        public bool isComplete;
        public int progress;
    }

}
