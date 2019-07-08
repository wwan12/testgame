using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 提供给其他具体实现建筑继承
/// </summary>
public abstract class BuildControl : MonoBehaviour
{
    [HideInInspector]
    public float durable;
    [HideInInspector]
    public float dTime;
    [HideInInspector]
    public float buildTime;
    [HideInInspector]
    public GameObject buildProgress;
    [HideInInspector]
    public GameObject dismantleProgress;
    [HideInInspector]
    public BuildingSO.BuildType type;
    /// <summary>
    /// 建筑价格
    /// </summary>
    [HideInInspector]
    public Dictionary<string, int> cost;
    /// <summary>
    /// 如果是收集建筑，可以收集的资源类型
    /// </summary>
    [HideInInspector]
    public ResourceType.AttributionType colType;
    /// <summary>
    /// 如果是收集建筑，可以收集的资源数
    /// </summary>
    [HideInInspector]
    public int colNum;
    private float progress = 0;
    private bool ready;
 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseOver()
    {
        if (ready&&Input.GetKeyDown(KeyCode.Mouse0)&&FindObjectOfType<PlayerManage>().InOperationRange(gameObject.transform.position))
        {
            Left();
           // Debug.LogWarning("l"+ready);
        }
        if (ready&&Input.GetKeyDown(KeyCode.Mouse1) && FindObjectOfType<PlayerManage>().InOperationRange(gameObject.transform.position))
        {
            Right();                         
        }
    }
    
    /// <summary>
    /// 当呗左键点中
    /// </summary>
    public abstract void Left();
    

    /// <summary>
    /// 当呗右键点中
    /// </summary>
    public abstract void Right();

    /// <summary>
    /// 建造完成
    /// </summary>
    public abstract void OnBuild();
    /// <summary>
    /// 拆除
    /// </summary>
    public abstract void OnDismantle();

    /// <summary>
    ///  不可用回调
    /// </summary>
    public abstract void OnNotAvailable();

    /// <summary>
    /// 可用回调
    /// </summary>
    public abstract void OnAvailable();

    /// <summary>
    /// todo 连接电力
    /// </summary>
    /// <returns></returns>
    public bool TryConnect()
    {
        return true;
    }

    public void RemoveBuild()
    {
        if (dTime != 0)
        {
            StartCoroutine(RemoveProgress());
        }
        else
        {
            RemoveComplete();
            Destroy(gameObject);
        }
    }

    IEnumerator RemoveProgress()
    {
        GameObject dp= GameObject.Instantiate(dismantleProgress);
        dp.transform.SetParent(AppManage.Instance.HUD.transform,false);
        Vector3 vector = gameObject.transform.position;
        vector.y += 0.2f;
        dp.transform.position = Camera.main.WorldToScreenPoint(vector);
        dp.AddComponent<BuildProgress>().lockBuild=gameObject;
        dp.transform.SetAsFirstSibling();
        while (progress < 100)
        {
            yield return new WaitForSeconds(dTime / 100);
            progress++;
            dp.GetComponent<Image>().fillAmount = progress / 100;
        }
        progress = 0;
        RemoveComplete();
        Destroy(dp);
        Destroy(gameObject);     
    }

    public void Build() {

        Messenger.Broadcast<Dictionary<string, int>>(EventCode.RESOURCE_REDUCE, cost);

        if (type==BuildingSO.BuildType.collect)
        {
            float x = transform.localScale.x * GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            float y = transform.localScale.y * GetComponent<SpriteRenderer>().sprite.bounds.size.y;
            colNum= GameObject.FindObjectOfType<MapManage>().QueryResource(gameObject.transform.position,colType,(int)x+1,(int)y+1);
            GetComponent<ResourceComponent>().Add(colType,colNum);
           // Messenger.AddListener<Vector3, ResourceType.AttributionType, Vector2Int, int>(EventCode);
        }
        if (buildTime != 0)
        {
            StartCoroutine(BuildProgress());
        }
        else
        {
            ready = true;
            BuildComplete();
        }
    }
   

    IEnumerator BuildProgress()
    {
        GameObject bp= GameObject.Instantiate(buildProgress);
        bp.transform.SetParent(AppManage.Instance.HUD.transform, false);
        Vector3 vector = gameObject.transform.position;
        vector.y += 0.2f;
        bp.transform.position= Camera.main.WorldToScreenPoint(vector);
        bp.AddComponent<BuildProgress>().lockBuild=gameObject;
        bp.transform.SetAsFirstSibling();
        Color alpha= gameObject.GetComponent<SpriteRenderer>().color;
        while (progress < 100)
        {         
            yield return new WaitForSeconds(buildTime / 100);
            progress++;
            alpha.a = progress / 100f * 255f;
            gameObject.GetComponent<SpriteRenderer>().color = alpha;
            bp.GetComponent<Image>().fillAmount = progress / 100;          
        }
        progress = 0;
        ready = true;
        Destroy(bp);
        BuildComplete();
    }

    void BuildComplete()
    {
        string position = gameObject.transform.position.x + "|" + gameObject.transform.position.y;     
        AppManage.Instance.saveData.buildLocation.Add(position,gameObject.name);
        OnBuild();
    }

    void RemoveComplete()
    {
        string position = gameObject.transform.position.x + "|" + gameObject.transform.position.y;      
        AppManage.Instance.saveData.buildLocation.Remove(position);
        OnDismantle();
    }
}
