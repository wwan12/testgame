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
    /// 
    /// </summary>
    /// <param name="prefabOnButton"></param>
    public void OnPlaceable(BuildingSO prefabOnButton)
    {

        if (placeable == null)
        {

            SetPlaceable(prefabOnButton);
        }
    }

    public void SetPlaceable(BuildingSO placeablePrefab)
    {
        //place = placeablePrefab.GetComponent<PlaceControl>();
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        placeable = Instantiate(placeablePrefab.buildingPrefab, mousePosition, Quaternion.identity);
        placeable.GetComponent<CircleCollider2D>().isTrigger = true;
        placeable.GetComponent<CircleCollider2D>().radius -= placeable.GetComponent<CircleCollider2D>().radius / 10;
        place = placeable.AddComponent<PlaceControl>();
        BuildControl build = placeable.AddComponent<BuildControl>();
        build.durable = placeablePrefab.durable;
        build.buildProgress = buildProgress;
        build.dismantleProgress = dismantleProgress;
        build.dTime = placeablePrefab.dTime;
        build.buildTime = placeablePrefab.buildTime;
        build.Build();
        // prefabToPlace = placeablePrefab;
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
        Messenger.AddListener<BuildingSO>(EventCode.BUILD_THIS,SetPlaceable);
       
    }

    // Update is called once per frame
    void Update()
    {

        // Have the object follow the mouse
        if (placeable != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            mousePosition = tileGrid.GetCellCenterWorld(tileGrid.WorldToCell(mousePosition));
            placeable.transform.position = mousePosition;
        }

        // Code to detach the object once you click the mouse
        if (Input.GetMouseButtonDown(0) && placeable != null )
        {
            if (place.isCheck)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;
                mousePosition = tileGrid.GetCellCenterWorld(tileGrid.WorldToCell(mousePosition));

                //  commandManager.SpawnPlacebable(prefabToPlace, mousePosition);
                placeable.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                placeable.GetComponent<CircleCollider2D>().isTrigger = false;
                placeable.GetComponent<SpriteRenderer>().color = Color.white;
                Destroy(placeable.GetComponent<PlaceControl>());
                placeable = null;
                // prefabToPlace = null;
                place = null;
            }

        }
        else if (Input.GetMouseButtonDown(1))
        {
            Destroy(placeable);
            placeable = null;
            prefabToPlace = null;
        }
    }


}
