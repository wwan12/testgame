using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("节点属性")]
    [Tooltip("下一个节点")]
    public NodeUI[] nextNodes;
    [Tooltip("前提的节点")]
    public NodeUI[] lastNodes;
    
    [Tooltip("完成的时间")]
    public float completeTime=1.0f;
    [Header("节点信息")]
    [Tooltip("节点名称")]
    public string nodeName;
    [Tooltip("节点说明")]
    public string nodeNote;
    [Tooltip("节点完成事件")]
    public UnityEvent passEvent;
    [HideInInspector]
    public RectTransform canvas;
    private bool isPass;
    private float hoverTimer = 0f; // 鼠标悬停时间
    private GameObject tooltips = null; // 属性面板的唯一引用

    private float timer = 0f;
    private bool pointEntered = false;
    private Vector2 offset; // Tooltips 面板偏移量
    private Image musk;
    private IEnumerator nodeTask;
    [HideInInspector]
    public float lineWidth=4f;
    [HideInInspector]
    public Image lineImage;
    [HideInInspector]
    public GameObject itemInfoPanel;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, -gameObject.GetComponent<RectTransform>().sizeDelta.y);
        musk = gameObject.GetComponent<Image>();
        NextLinksPaths();
    }

    // Update is called once per frame
    void Update()
    {
        if (pointEntered && timer <= hoverTimer)
        {
            timer += Time.deltaTime;
            if (timer > hoverTimer)
            {
                PopupToolTips();
            }
        }
    }



    void NextLinksPaths()
    {
        foreach (var node in nextNodes)
        {           
            float startX = gameObject.transform.position.x ;
            float startY = gameObject.transform.position.y  ;
            float endX = node.gameObject.transform.position.x;
            float endY = node.gameObject.transform.position.y;
            List<Vector3> paths = new List<Vector3>();
            if (startX == endX)
            {
                paths.Add(new Vector3(startX, startY,0));//起点
                paths.Add(new Vector3(endX, endY,0));//结束点
            }
            else
            {
               
                paths.Add(new Vector3(startX, - Screen .height+ startY,0));//起点
                paths.Add(new Vector3(startX,-Screen.height+( startY +endY)/2, 0));//中继点a
                paths.Add(new Vector3(endX,-Screen.height + (startY + endY) / 2, 0));//中继点b
                paths.Add(new Vector3(endX,- Screen.height+endY, 0));//结束点
            }
             DrawPaths(paths);
        }

    }


    void DrawPaths(List<Vector3> paths)
    {
        for (int i = 0; i < paths.Count-1; i++)
        {
            lineImage = GameObject.Instantiate(Resources.Load<GameObject>("prefabs/Node/LineImage")).GetComponent<Image>();         
            if (paths[i].y - paths[i + 1].y==0)//判断画的是横线还是竖线
            {
                lineImage.transform.position = new Vector2(paths[i].x - (paths[i].x - paths[i + 1].x)/2, paths[i].y);
                lineImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Abs(paths[i].x - paths[i + 1].x)+lineWidth, lineWidth);
            }
            else
            {
                lineImage.transform.position = new Vector2(paths[i].x, paths[i].y - (paths[i].y - paths[i + 1].y) / 2);
                lineImage.GetComponent<RectTransform>().sizeDelta = new Vector2(lineWidth,Mathf.Abs(paths[i].y - paths[i + 1].y));
            }         
            lineImage.transform.SetParent(canvas, false);
            lineImage.transform.SetAsFirstSibling();
            //line= GameObject.Instantiate<GameObject>(line);
        }

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isPass)
            {
                //todo 通知+音效
                SendErrorSound();
            }
            else
            {
                foreach (var node in lastNodes)
                {
                    if (!node.isPass)
                    {
                        //todo 通知+音效
                        SendErrorSound();
                        return;
                    }
                }
                nodeTask = StartNode();
                StartCoroutine(nodeTask);
                
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            //todo对话框
            StopCoroutine(nodeTask);
        }
    }

    void SendErrorSound()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/NodeError");
        AudioManage.Instance.PushClip(clip);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        pointEntered = true;
        if (eventData.dragging)
        {
            if (tooltips != null)
            {
                //  tooltips.SetActive(false);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointEntered = false;
        timer = 0f;
        if (tooltips != null)
        {
            tooltips.SetActive(false);
        }
    }
    public void PopupToolTips()
    {
        Vector3 initPos = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z);
        if (tooltips == null)
        {
            tooltips = GameObject.Instantiate(itemInfoPanel, initPos, Quaternion.identity);
            tooltips.transform.SetParent(canvas);
        }
        tooltips.transform.position = initPos;
        tooltips.AddComponent<AddTextInfo>().AddInfo(nodeName);
        tooltips.AddComponent<AddTextInfo>().AddInfo(nodeNote);
        tooltips.SetActive(true);
    }

    IEnumerator StartNode()
    {
        for (int i = 1; i < 101; i++)
        {
            yield return new WaitForSeconds(completeTime / 100f);
            musk.fillAmount = i / 100f;
        }
        AudioClip clip = Resources.Load<AudioClip>("Sounds/NodeComplete");
        AudioManage.Instance.PushClip(clip);
        passEvent.Invoke();
    }


   
}
