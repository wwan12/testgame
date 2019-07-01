using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologyManage : MonoBehaviour
{
    [Tooltip("说明面板")]
    public GameObject itemInfoPanel;
    public event EventHandler<TechnologyInfo> TechnologyUpCallBack;
    // Start is called before the first frame update
    void Start()
    {
        CanvasGroup group = gameObject.AddComponent<CanvasGroup>();
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class TechnologyInfo
    {
        public string name;
       
    }


}
