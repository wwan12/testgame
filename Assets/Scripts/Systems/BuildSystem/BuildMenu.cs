using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenu : MonoBehaviour
{
    [Tooltip("分成几栏")]
    public int hierarchy;
    [Tooltip("是否从磁盘读取")]
    public bool isOSRead=true;
    [Tooltip("归属的页签")]
    public string tagName;
    private readonly string OS_PATH = "Assets/BuildAssets/"; 
    [Tooltip("建筑UI节点s")]
    public GameObject[] allBuild;
    [Tooltip("上下间隔")]
    public int interval=20;

    private GameObject[] hierarchys;

    public static GameObject board;
    /// <summary>
    /// 全局激活建筑
    /// </summary>
    public static void AvailableNode(BuildingSO build)
    {
        if (GameObject.FindObjectOfType<BuildMenu>() == null)
        {
            AppManage.Instance.saveData.buildNodes[build.hierarchy][build.objectName] = true;
        }
        else
        {
            GameObject.FindObjectOfType<BuildMenu>().SetAvailable(build.objectName);
        }

    }
    


    // Start is called before the first frame update
    void Start()
    {
        if (isOSRead)
        {
           BuildingSO[] allBuildOS= Resources.LoadAll<BuildingSO>(OS_PATH+ tagName);
            allBuild = new GameObject[allBuildOS.Length];
            for (int i = 0; i < allBuildOS.Length; i++)
            {
                GameObject build= Resources.Load<GameObject>("prefabs/UI/BuildNode");
                //  GameObject build = new GameObject("Node"+i);
                build.GetComponent<BuildNode>().building = allBuildOS[i];
                build = GameObject.Instantiate<GameObject>(build);
         
                // node.building = allBuildOS[i];
                allBuild[i] = build;
            }
          
        }
        float lx=180;
        float ly = -gameObject.GetComponent<RectTransform>().rect.y * 2;
        if (allBuild.Length>0)
        {
           lx= allBuild[0].GetComponent<RectTransform>().sizeDelta.x+2*20;
        }
        hierarchys = new GameObject[hierarchy];

        for (int i = 0; i < hierarchy; i++)
        {
            GameObject l = new GameObject("Hierarchy"+i);
           // l = GameObject.Instantiate<GameObject>(l);
            RectTransform rect= l.AddComponent<RectTransform>();
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, lx*i+100, lx);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0,ly);
            VerticalLayoutGroup vertical = l.AddComponent<VerticalLayoutGroup>();          
            vertical.childAlignment = TextAnchor.LowerLeft;           
            vertical.childControlWidth = false;
            vertical.childControlHeight = false;
            //rect.sizeDelta= new Vector2(lx, 1080);
            l.transform.SetParent(gameObject.transform,false);
            hierarchys[i] = l;
        }

        for (int i = 0; i < allBuild.Length; i++)
        {
            GameObject h = hierarchys[allBuild[i].GetComponent<BuildNode>().hierarchy];
            VerticalLayoutGroup vertical = h.GetComponent<VerticalLayoutGroup>();
            int top = (int)((h.transform.childCount + 1) * allBuild[i].GetComponent<RectTransform>().sizeDelta.y);
            int il = (h.transform.childCount + 1) * interval;
            vertical.padding = new RectOffset(20, 0, (int)ly - top - 20 - il, 20);
            if (!isOSRead)
            {
                allBuild[i] = GameObject.Instantiate<GameObject>(allBuild[i]);
            }          
            allBuild[i].transform.SetParent(h.transform);
        }
        if (AppManage.Instance.isInGame)
        {
            ReadNodes(AppManage.Instance.saveData.buildNodes);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  

    public void SetAvailable(string buildName )
    {
        foreach (var build in allBuild)
        {
            BuildNode node= build.GetComponent<BuildNode>();
            if (node.building.objectName.Equals(buildName))
            {
                node.SetAvailable();
            }
            
        }
    }


    /// <summary>
    /// 储存节点允许状态
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, Dictionary<string, bool>> SaveNodes()
    {
        Dictionary<int, Dictionary<string, bool>> nodes = new Dictionary<int, Dictionary<string, bool>>();
        for (int i = 0; i < hierarchys.Length; i++)
        {
            Dictionary<string, bool> pairs = new Dictionary<string, bool>();
            foreach (var node in hierarchys[i].GetComponentsInChildren<Transform>())
            {
                BuildNode buildNode= node.gameObject.GetComponent<BuildNode>();
                pairs.Add(buildNode.building.objectName, buildNode.isBuild);
                
            }
            nodes.Add(i,pairs);
        }
        return nodes;

    }
    /// <summary>
    /// 恢复节点允许状态
    /// </summary>
    /// <param name="pairs"></param>
    public void ReadNodes(Dictionary<int, Dictionary<string, bool>> pairs)
    {
        if (pairs==null)
        {
            return;
        }
        foreach (var pair in pairs)
        {
            foreach (var p in pair.Value)
            {
                foreach (var build in allBuild)
                {
                    BuildNode buildNode= build.GetComponent<BuildNode>();
                    if (buildNode.building.objectName.Equals(p.Key))
                    {
                        if (p.Value)
                        {
                            buildNode.SetAvailable();
                        }
                      
                    }
                    
                }
            }
        }

    }


  
}
