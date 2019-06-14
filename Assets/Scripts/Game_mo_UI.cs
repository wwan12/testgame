﻿using DialogueManager;
using External;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_mo_UI : MonoBehaviour
{

#if UNITY_EDITOR
    public BuildingSO Building;
#endif

    // Start is called before the first frame update
    void Start()
    {

        //Physics.gravity = new Vector3(0, -1.0F, 0);
        ExternalRead read = new ExternalRead();
        read.ReadItems(this);

#if UNITY_EDITOR
        GameObject.FindObjectOfType<MapManage>().CreateMap();
        gameObject.AddComponent<FPSOnGUIText>();
       
#endif
    }



    private void OnGUI()
    {
#if UNITY_EDITOR
        GUI.Box(new Rect(30, 200, 200, 500), "Menu");
        if (GUI.Button(new Rect(50, 250, 100, 40), "添加物品")) {
            GameObject.FindGameObjectWithTag("Bag").GetComponent<BagManage>().BagAddItem(new ItemInfo()
            {
                name = "aaa" + 0,
                sprite = Resources.Load<Sprite>("Texture/Items/zdnp"),
                num = 12,
            });
        }
        if (GUI.Button(new Rect(50, 300, 100, 40), "扩大地图"))
        {
            GameObject.FindObjectOfType<MapManage>().ExpandMap();
        }
        if (GUI.Button(new Rect(50, 350, 100, 40), "播放对话"))
        {
            GameObject.FindObjectOfType<PlayerManage>().gameObject.GetComponents<ConversationComponent>()[0].Trigger();
        }
        if (GUI.Button(new Rect(50, 400, 100, 40), "放置测试建筑"))
        {
            GameObject.FindObjectOfType<SpawnBuildings>().SpawnBuilding(Building);
        }
        if (GUI.Button(new Rect(50, 450, 100, 40), "放置测试敌人"))
        {
            
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    

}
