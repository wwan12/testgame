using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 放置控制
/// </summary>
public class PlacingManage : MonoBehaviour {

    private GameObject placeable = null;
    private GameObject prefabToPlace = null;
   // private ServerCommands commandManager = null;
    [Tooltip("建筑建造的坐标系")]
    public Grid tileGrid = null;
    [Tooltip("todo 建造时显示")]
    public GameObject building;
    [Tooltip("建造进度条")]
    public GameObject buildProgress;
    [Tooltip("拆除进度条")]
    public GameObject dismantleProgress;
    private PlaceControl place = null;

    /// <summary>
    /// 准备建造
    /// </summary>
    /// <param name="prefabOnButton"></param>
    public void OnPlaceable(BuildingSO prefabOnButton)
    {
       
        if (placeable == null)
        {
            AppManage.Instance.CloseOpenUI();
           
            SetPlaceable( prefabOnButton);
        }
    }


    /// <summary>
    /// 在鼠标位置初始化未建造的建筑
    /// </summary>
    /// <param name="placeablePrefab"></param>
    public void SetPlaceable( BuildingSO placeablePrefab)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        //place = placeablePrefab.GetComponent<PlaceControl>();       
        placeable = Instantiate(placeablePrefab.buildingPrefab, mousePosition, Quaternion.identity);
        placeable.AddComponent<Rigidbody2D>();
        BoxCollider2D boxCollider2D = placeable.AddComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
        boxCollider2D.size = new Vector2(boxCollider2D.size.x-boxCollider2D.size.x/20,boxCollider2D.size.y- boxCollider2D.size.y/20);
        
        place = placeable.AddComponent<PlaceControl>();
        BuildControl build = placeable.GetComponent<BuildControl>();
        placeable.name = placeablePrefab.objectName;
        build.sprite = placeablePrefab.lowSource;
        build.name = placeablePrefab.objectName;
        build.durable = placeablePrefab.durable;
        build.buildProgress = buildProgress;
        build.dismantleProgress = dismantleProgress;
        build.dTime = placeablePrefab.dTime;
        build.buildTime = placeablePrefab.buildTime;
        build.type = placeablePrefab.type;
        build.cost = placeablePrefab.cost;
        build.colType = placeablePrefab.res;
        build.colNum = placeablePrefab.collectNum;
     

        // prefabToPlace = placeablePrefab;
    }


    /// <summary>
    /// 在地图上设置建造完的建筑
    /// </summary>
    /// <param name="placeablePrefab"></param>
    public void SetPlaceable(Vector3 initPosition, BuildingSO placeablePrefab)
    {
        //place = placeablePrefab.GetComponent<PlaceControl>();       
        GameObject place = Instantiate(placeablePrefab.buildingPrefab, initPosition, Quaternion.identity);
        placeable.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        BoxCollider2D boxCollider2D = placeable.GetComponent<BoxCollider2D>();
      //  boxCollider2D.isTrigger = true;
        boxCollider2D.size = new Vector2(boxCollider2D.size.x - boxCollider2D.size.x / 20, boxCollider2D.size.y - boxCollider2D.size.y / 20);
        BuildControl build = placeable.GetComponent<BuildControl>();
        placeable.name = placeablePrefab.objectName;
        build.name = placeablePrefab.objectName;
        build.durable = placeablePrefab.durable;
        build.buildProgress = buildProgress;
        build.dismantleProgress = dismantleProgress;
        build.dTime = placeablePrefab.dTime;
        build.buildTime = 0;
        build.type = placeablePrefab.type;
        build.cost = null;
        build.Build();
        // prefabToPlace = placeablePrefab;
    }
    /// <summary>
    /// 恢复已建造的建筑
    /// </summary>
    /// <param name="saveBuilds"></param>
    public void RecoveryBuilds(Dictionary<string, string> saveBuilds)
    {
        foreach (var b in saveBuilds)
        {
            string[] position = b.Key.Split('|');
            Vector3 vector = new Vector3(float.Parse(position[0]), float.Parse(position[1]), 0);
            SetPlaceable(vector, Resources.Load<BuildingSO>("Assets/BuildAssets/" + b.Value));

        }
    }

    void StartInit(AppManage.SingleSave save)
    {
        RecoveryBuilds(save.buildLocation);
    }

    //  public void SetCommandManager(ServerCommands manager)
    //  {
    //      commandManager = manager;
    //  }

    // Use this for initialization
    void Start()
    {
        if (tileGrid==null)
        {
            tileGrid =  GameObject.FindObjectOfType<Grid>();
        }
        Messenger.AddListener<BuildingSO>(EventCode.BUILD_THIS, OnPlaceable);
        Messenger.AddListener<AppManage.SingleSave>(EventCode.APP_START_GAME, StartInit);
    }

    Vector3 cachePosition=new Vector3();

    // Update is called once per frame
    void Update()
    {

        // Have the object follow the mouse
        if (placeable != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition = tileGrid.GetCellCenterWorld(tileGrid.WorldToCell(mousePosition));
            mousePosition.z = 0;
            if (!cachePosition.Equals(mousePosition))
            {
                cachePosition = mousePosition;
                placeable.transform.position = mousePosition;
            }
            
        }

        // Code to detach the object once you click the mouse
        if (Input.GetMouseButtonDown(0) && placeable != null )
        {
            if (place.isCheck)
            {
                //检测是否有资源
             
                bool r = Messenger.BroadcastReturn<Dictionary<string, int>, bool>(EventCode.RESOURCE_CHECK, placeable.GetComponent<BuildControl>().cost);
                StartBuild(r);


                //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //mousePosition.z = 0;
                //mousePosition = tileGrid.GetCellCenterWorld(tileGrid.WorldToCell(mousePosition));
                //  commandManager.SpawnPlacebable(prefabToPlace, mousePosition);

            }

        }
        else if (Input.GetMouseButtonDown(1))
        {
            Destroy(placeable);
            placeable = null;
            prefabToPlace = null;
        }
    }

    void StartBuild(bool r)
    {
        if (r)
        {
            placeable.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            placeable.GetComponent<BoxCollider2D>().isTrigger = false;
            placeable.GetComponent<SpriteRenderer>().color = Color.white;
            placeable.GetComponent<BuildControl>().Build();

            Destroy(placeable.GetComponent<PlaceControl>());
            placeable = null;
            // prefabToPlace = null;
            place = null;
        }
        else
        {
            Messenger.Broadcast<string>(EventCode.AUDIO_EFFECT_PLAY, AudioCode.SYSTEM_ERROR);
            
        }

    }
}
