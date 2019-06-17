using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SpawnBuildings : MonoBehaviour
{
    [Tooltip("放置建筑的网格组件")]
    public GameObject productionTile;

    [Tooltip("地形所在的图层")]
    public LayerMask terrainLayer;

    [Tooltip("需要GraphicRaycaster检测单击按钮")]
    public GraphicRaycaster uiRaycaster;
    [Tooltip("建造过程中显示的模型")]
    public GameObject underConstructionGO;
    private BuildingSO buildingToPlace;


    GameObject currentSpawnedBuilding;
    RaycastHit hit;
    List<ProductionTile> activeTiles;
    GameObject activeTilesParent;


    void Start ()
    {
        activeTiles = new List<ProductionTile>();
      
	}


    void Update()
    {
        if (currentSpawnedBuilding)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!PlacementHelpers.RaycastFromMouse(out hit, terrainLayer))
                    return;

                currentSpawnedBuilding.transform.position = hit.point;

                if(CanPlaceBuilding())
                    PlaceBuilding();
            }
            if (Input.GetMouseButtonDown(1))
                Destroy(currentSpawnedBuilding);
        }
    }


    void FixedUpdate()
    {
        if(currentSpawnedBuilding)
            if(PlacementHelpers.RaycastFromMouse(out hit, terrainLayer))
                currentSpawnedBuilding.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
    }


    bool CanPlaceBuilding()
    {
        if (PlacementHelpers.IsButtonPressed(uiRaycaster))
            return false;
        for(int i = 0; i < activeTiles.Count; i++)
            if(activeTiles[i].colliding)
                return false;
        return true;
    }

    /// <summary>
    /// 启动建筑过程
    /// </summary>
    void PlaceBuilding()
    {
        ClearGrid();
        StartCoroutine(BeginBuilding());
    }


    void ClearGrid()
    {
        Destroy(activeTilesParent);
        activeTiles.RemoveAll(i => i);
    }


    IEnumerator BeginBuilding()
    {
        Vector3 pos = currentSpawnedBuilding.transform.position;
        GameObject instance = currentSpawnedBuilding;
        currentSpawnedBuilding = null;

        RaycastHit hitTerrain;
        if (PlacementHelpers.RaycastFromMouse(out hitTerrain, terrainLayer))
            pos = hitTerrain.point;
        underConstructionGO.GetComponent<ShowBuildProgress>().buildingToPlace = buildingToPlace;
        GameObject go = Instantiate(underConstructionGO, pos, Quaternion.identity);
        yield return new WaitForSeconds(buildingToPlace.buildTime);
        PlacementHelpers.ToggleRenderers(instance, true);
        Destroy(go);
    }

    /// <summary>
    /// 计算建筑的碰撞体
    /// </summary>
    /// <param name="col"></param>
    void FillRectWithTiles(Collider2D col)
    {
        if (activeTilesParent)
            return;

        Rect rect = PlacementHelpers.MakeRectOfCollider(col);
        float fromX = rect.position.x;
        float toX = (rect.position.x + rect.width) * col.gameObject.transform.localScale.x;
        float fromZ = rect.position.y;
        float toZ = (rect.position.y + rect.height) * col.gameObject.transform.localScale.y;

        GameObject parent = new GameObject("PlacementGrid");
        parent.transform.SetParent(col.gameObject.transform.root);
        parent.transform.position = col.gameObject.transform.InverseTransformPoint(new Vector3(0, 0.5f, 0));

        for(float i = -toX/2; i <= toX/2; i += productionTile.transform.localScale.x)
        {
            for(float j = -toZ/2; j <= toZ/2; j += productionTile.transform.localScale.y)
            {
                GameObject tile = Instantiate(productionTile);
                tile.transform.SetParent(parent.transform);
                tile.transform.position = new Vector3(i, parent.transform.position.y, j);
                activeTiles.Add(tile.GetComponent<ProductionTile>());
            }
        }
        activeTilesParent = parent;
    }

    /// <summary>
    /// 开始放置建筑
    /// </summary>
    /// <param name="building"></param>
    public void SpawnBuilding(BuildingSO building)
    {
        // 如果没有放置建筑，则返回
        if (currentSpawnedBuilding)
            return;

        currentSpawnedBuilding = Instantiate(building.buildingPrefab);
        buildingToPlace = building;
        PlacementHelpers.ToggleRenderers(currentSpawnedBuilding, false);

        Collider2D cols = currentSpawnedBuilding.GetComponent<Collider2D>();
        if(cols!=null)
            FillRectWithTiles(cols);
    }
}
