using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechnologyMenu : MonoBehaviour
{
    [Tooltip("分成几栏")]
    public int hierarchy;
    [Tooltip("上下间隔")]
    public int interval = 20;

    private GameObject[] hierarchys;
    private GameObject[] allTechnology;
    private int researchProgress;
    /// <summary>
    /// 效率
    /// </summary>
    private float researchEfficiency;
    public bool isTec;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GameObject.FindObjectOfType<TechnologyManager>().allTechnology.Length; i++)
        {
            GameObject Technology = Resources.Load<GameObject>("prefabs/UI/TechnologyNode");
            //  GameObject Technology = new GameObject("Node"+i);          
            Technology.GetComponent<TechnologyNode>().tec = GameObject.FindObjectOfType<TechnologyManager>().allTechnology[i];
            // Technology.GetComponent<TechnologyNode>().menu = this;
             Technology = GameObject.Instantiate<GameObject>(Technology);
            // node.Technologying = allTechnologyOS[i];
            allTechnology[i] = Technology;
        }
      
        
        float lx = 180;
        float ly = -gameObject.GetComponent<RectTransform>().rect.y * 2;
        if (allTechnology.Length > 0)
        {
            lx = allTechnology[0].GetComponent<RectTransform>().sizeDelta.x + 2 * 20;
        }
        hierarchys = new GameObject[hierarchy];

        for (int i = 0; i < hierarchy; i++)
        {
            GameObject l = new GameObject("Hierarchy" + i);
            // l = GameObject.Instantiate<GameObject>(l);
            RectTransform rect = l.AddComponent<RectTransform>();
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, lx * i, lx);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, ly);
            //rect.sizeDelta= new Vector2(lx, 1080);
            l.transform.SetParent(gameObject.transform, false);
            hierarchys[i] = l;
        }

        for (int i = 0; i < allTechnology.Length; i++)
        {
            GameObject h = hierarchys[allTechnology[i].GetComponent<TechnologyNode>().hierarchy];
            //   h = GameObject.Instantiate<GameObject>(h);
            VerticalLayoutGroup vertical = h.AddComponent<VerticalLayoutGroup>();
            vertical.childAlignment = TextAnchor.LowerLeft;
            int top = (int)((h.transform.childCount + 1) * allTechnology[i].GetComponent<RectTransform>().sizeDelta.y);
            int il = (h.transform.childCount + 1) * interval;
            vertical.padding = new RectOffset(20, 0, (int)ly - top - 20 - il, 20);
            vertical.childControlWidth = false;
            vertical.childControlHeight = false;
           // allTechnology[i] = GameObject.Instantiate<GameObject>(allTechnology[i]);            
            allTechnology[i].transform.SetParent(h.transform);
        }
        if (AppManage.Instance.isInGame)
        {
            ReadNodes(FindObjectOfType<TechnologyManager>().allTechnology);
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
        foreach (var Technology in allTechnology)
        {
            TechnologyNode node = Technology.GetComponent<TechnologyNode>();
            if (node.name.Equals(TechnologyName))
            {
                node.SetAvailable();
            }

        }
    }
 

    public void StartTec(string tecName)
    {
        foreach (var tec in allTechnology)
        {
            if (tec.name.Equals(tecName))
            {
                StartTec(tec.GetComponent<TechnologyNode>());
            }
        }
      
    }

    public void StartTec(TechnologyNode tec)
    {
        isTec = true;
        tec.tec.technologyControl.OnStart();
        StartCoroutine(TecProgress(tec));
    }

    IEnumerator TecProgress(TechnologyNode tec)
    {
        Image progress=null;
        foreach (var item in tec.gameObject.GetComponentsInChildren<Image>())
        {
            if (item.name.Equals("Progress"))
            {
                progress = item;
            }
        }
         
        while (true)
        {
            yield return new WaitForSeconds(tec.tec.researhTime / 100);
            researchProgress++;
            progress.fillAmount = researchProgress/100;
            tec.tec.progress = researchProgress;
            if (researchProgress >= 100)
            {
                //todo 研究完成
                tec.tec.technologyControl.OnComplete(this);
                Messenger.Broadcast(EventCode.AUDIO_EFFECT_PLAY,AudioCode.SYSTEM_COMPLETE);
                tec.tec.isComplete = true;
                tec.tec.progress = 0;
                researchProgress = 0;
                isTec = false;
                break;
            }
        }
       
    }

    /// <summary>
    /// 恢复节点允许状态 
    /// </summary>
    /// <param name="pairs"></param>
    public void ReadNodes(Technology[] pairs)
    {
        if (pairs == null)
        {
            return;
        }
        foreach (var pair in pairs)
        {

            foreach (var Technology in allTechnology)
            {
                TechnologyNode TechnologyNode = Technology.GetComponent<TechnologyNode>();
                if (TechnologyNode.name.Equals(pair.name))
                {
                    if (pair.isResearch)
                    {
                        TechnologyNode.SetAvailable();
                    }
                    if (pair.isComplete)
                    {
                        TechnologyNode.SetComplete();
                    }
                    if (pair.progress != 0)
                    {
                        StartTec(pair.name);
                    }
                }

            }

        }

    }

}
