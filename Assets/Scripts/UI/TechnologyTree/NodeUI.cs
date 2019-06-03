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
    [Tooltip("物品信息面板")]
    public GameObject itemInfoPanel;
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
    public NodeInfo info;
    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, -gameObject.GetComponent<RectTransform>().sizeDelta.y);
        musk = gameObject.GetComponent<Image>();
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
            Vector3 vector = (gameObject.transform.position + node.gameObject.transform.position)/2;
            float width= gameObject.GetComponent<RectTransform>().sizeDelta.x;
            float height = gameObject.GetComponent<RectTransform>().sizeDelta.y;
            float startX = gameObject.transform.position.x + width / 2;
            float startY = gameObject.transform.position.y + height / 2;
            float endX = node.gameObject.transform.position.x + width / 2;
            float endY = node.gameObject.transform.position.y - height / 2;
            List<Vector3> paths = new List<Vector3>(4);
            if (startX == endX)
            {
                paths.Add(new Vector3(startX, startY));//起点
                paths.Add(new Vector3(endX, endY));//结束点
            }
            else
            {
                paths.Add(new Vector3(startX, startY));//起点
                paths.Add(new Vector3(vector.x + width / 2, vector.y));//中继点a
                paths.Add(new Vector3(vector.x + width / 2, vector.y));//中继点b
                paths.Add(new Vector3(endX, endY));//结束点
            }
          
        }

    }

    void DrawPaths(List<Vector3> paths)
    {

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
       // tooltips.GetComponent<ItemInfoPanel>().SetInfoPanel(info);
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
