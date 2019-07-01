using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenu : MonoBehaviour
{
    [Tooltip("分成几栏")]
    public int hierarchy;
    [Tooltip("是否从磁盘读取")]
    public bool isOSRead;

    private readonly string OS_PATH = "Assets/BuildAssets"; 
    [Tooltip("建筑UI节点s")]
    public GameObject[] allBuild;
    [Tooltip("上下间隔")]
    public int interval=20;

    private GameObject[] hierarchys;


    // Start is called before the first frame update
    void Start()
    {
        if (isOSRead)
        {
           BuildingSO[] allBuildOS= Resources.LoadAll<BuildingSO>(OS_PATH);
            allBuild = new GameObject[allBuildOS.Length];
            for (int i = 0; i < allBuildOS.Length; i++)
            {
                GameObject build= Resources.Load<GameObject>("prefabs/UI/BuildNode");
              //  GameObject build = new GameObject("Node"+i);
                build = GameObject.Instantiate<GameObject>(build);
                build.GetComponent<BuildNode>().building=allBuildOS[i];
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
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, lx*i, lx);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0,ly);
            //rect.sizeDelta= new Vector2(lx, 1080);
            l.transform.SetParent(gameObject.transform,false);
            hierarchys[i] = l;
        }

        for (int i = 0; i < allBuild.Length; i++)
        {
            GameObject h = hierarchys[allBuild[i].GetComponent<BuildNode>().hierarchy];
            //   h = GameObject.Instantiate<GameObject>(h);
            VerticalLayoutGroup vertical = h.AddComponent<VerticalLayoutGroup>();
            vertical.childAlignment = TextAnchor.LowerLeft;
            int top = (int)((h.transform.childCount + 1) * allBuild[i].GetComponent<RectTransform>().sizeDelta.y);
            int il = (h.transform.childCount + 1) * interval;
            vertical.padding = new RectOffset(20, 0, (int)ly - top - 20 - il, 20);
            vertical.childControlWidth = false;
            vertical.childControlHeight = false;
            if (!isOSRead)
            {
                allBuild[i] = GameObject.Instantiate<GameObject>(allBuild[i]);
            }          
            allBuild[i].transform.SetParent(h.transform);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
