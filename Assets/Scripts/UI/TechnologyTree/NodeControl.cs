using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeControl : MonoBehaviour
{
    private NodeUI[] allNodeUI;
    [Tooltip("连线宽度")]
    public float lineWidth=4f;
    [Tooltip("说明背景")]
    public GameObject itemInfoPanel;
    // Start is called before the first frame update
    void Start()
    {
        allNodeUI = GetComponentsInChildren<NodeUI>();
        RectTransform rt= gameObject.GetComponent<RectTransform>();
        foreach (var node in allNodeUI)
        {          
            node.canvas = rt;
            node.lineWidth = lineWidth;
            node.itemInfoPanel = itemInfoPanel;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
