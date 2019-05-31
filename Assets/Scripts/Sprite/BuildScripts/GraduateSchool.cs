using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraduateSchool : MonoBehaviour
{
    /// <summary>
    /// 研究进度回调
    /// </summary>
    public event EventHandler<int> progressCallBcak;
    /// <summary>
    /// 研究完成
    /// </summary>
    public event EventHandler<string> completeCallBcak;
    private float time;
    private byte progress=0;
    private bool isRun;
    [HideInInspector]
    public List<string> researched = new List<string>();
    [HideInInspector]
    public List<string> notResearch = new List<string>();
    private GameObject[] allTec;
    // Start is called before the first frame update
    void Start()
    {
        ReadTecTree();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 读取科技树
    /// </summary>
    void ReadTecTree()
    {
        allTec= Resources.LoadAll<GameObject>("prefabs/Technology");
    }

    void AddUI()
    {
       GameObject ui= GameObject.Find("TechnologyMenu");
        foreach (var tec in allTec)
        {
            RectTransform rect = tec.GetComponent<RectTransform>();
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,0 , rect.sizeDelta.x);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, rect.sizeDelta.y);
            tec.transform.SetParent(ui.transform);
        }
    }
    /// <summary>
    /// 直接获取一个科技
    /// </summary>
    /// <param name="tecName"></param>
    public void GetTec(string tecName)
    {
        researched.Add(tecName);
    }

     bool IsResearch()
    {
        return false;
    }
    /// <summary>
    /// 开始研究
    /// </summary>
    /// <param name="tecName"></param>
    public void StartResearch(string tecName)
    {
        if (!isRun)
        {
         StartCoroutine(Progress(tecName));        
        }
    }

    IEnumerator Progress(string tecName) {
        isRun = true;
        while (progress < 100)
        {
        yield return new WaitForSeconds(time/100);
        progress++;
        progressCallBcak(this, progress);
        }
        progress = 0;
        researched.Add(tecName);
        TecTakeEffect(tecName);
        completeCallBcak(this, tecName);
        isRun = false;
    }

    void TecTakeEffect(string tecName)
    {

    }
}
