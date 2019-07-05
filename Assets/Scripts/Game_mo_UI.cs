using DialogueManager;
using External;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 测试以及基础加载功能
/// </summary>
public class Game_mo_UI : MonoBehaviour
{

#if UNITY_EDITOR
    public BuildingSO testBuild;
    public GameObject testEnemy;
    public AutoGenerateControl testAutoGen;
#endif

    public Texture saveImage;
    private bool isSave;
    private int saveTime;
    // Start is called before the first frame update
    void Start()
    {
        //物品过多可能有开销问题
        //Physics.gravity = new Vector3(0, -1.0F, 0);
        ExternalRead read = new ExternalRead();
        read.ReadItems(this);
        
        Messenger.AddListener(EventCode.APP_SAVE_GAME,SaveShow);
        Messenger.AddListener(EventCode.APP_SAVEOVER_GAME, SaveOver);
#if UNITY_EDITOR
        Game_mo_UI ui = GameObject.FindObjectOfType<Game_mo_UI>();
        AppManage.Instance.HUD = ui.gameObject;
        // isSave = true;
        GameObject.FindObjectOfType<MapManage>().CreateMap();
       // GameObject.FindObjectOfType<MapManage>().ChangeTile();
        gameObject.AddComponent<FPSOnGUIText>();
       
#endif
    }

    void SaveShow()
    {
        isSave = true;
    }

    void SaveOver()
    {
        isSave = false;
    }


    private void OnGUI()
    {
        if (isSave)
        {
            if (saveTime<24)
            {
                GUIStyle style = new GUIStyle
                {
                    border = new RectOffset(10, 10, 10, 10),
                };
                GUI.Box(new Rect(50, Screen.height - 100, 100, 100), saveImage, style);
                saveTime++;
            }
            else
            {
                saveTime = 0;
            }                    
        }

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
            // GameObject.FindObjectOfType<SpawnBuildings>().SpawnBuilding(Building);
            GameObject.FindObjectOfType<PlacingManage>().tileGrid = GameObject.FindObjectOfType<Grid>();
            GameObject.FindObjectOfType<PlacingManage>().OnPlaceable(testBuild);
        }
        if (GUI.Button(new Rect(50, 450, 100, 40), "放置测试敌人"))
        {
            GameObject.Instantiate(testEnemy, GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0, 3, 0), Quaternion.identity);

        }
        if (GUI.Button(new Rect(50, 500, 100, 40), "放置刷新点"))
        {
            GameObject gameObject = GameObject.Instantiate(testAutoGen.gameObject, GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0, 3, 0), Quaternion.identity);
            gameObject.AddComponent<AutoGenerateControl>().StartGenerate();

        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    

}
