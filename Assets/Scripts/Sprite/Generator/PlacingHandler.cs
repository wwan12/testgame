using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 放置控制
/// </summary>
public class PlacingHandler : MonoBehaviour {

    private GameObject placeable = null;
    private GameObject prefabToPlace = null;
   // private ServerCommands commandManager = null;
    private Grid tileGrid = null;

    public void OnPlaceableButtonClicked(GameObject prefabOnButton)
    {

        if (placeable == null)
        {
            SetPlaceable(prefabOnButton);
        }
    }

    public void SetPlaceable(GameObject placeablePrefab)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        placeable = Instantiate(placeablePrefab, mousePosition, Quaternion.identity);
        prefabToPlace = placeablePrefab;
    }

    //public void SetCommandManager(ServerCommands manager)
    //{
    //    commandManager = manager;
    //}

	// Use this for initialization
	void Start ()
    {
        tileGrid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
	}
	
	// Update is called once per frame
	void Update ()
    {

        // Have the object follow the mouse
        if(placeable != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            mousePosition = tileGrid.GetCellCenterWorld(tileGrid.WorldToCell(mousePosition));
            placeable.transform.position = mousePosition;
        }

        // Code to detach the object once you click the mouse
        if(Input.GetMouseButtonDown(0) && placeable != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            mousePosition = tileGrid.GetCellCenterWorld(tileGrid.WorldToCell(mousePosition));

         //   commandManager.SpawnPlacebable(prefabToPlace, mousePosition);
            Destroy(placeable);
            placeable = null;
            prefabToPlace = null;
        }
        else if(Input.GetMouseButtonDown(1))
        {
            Destroy(placeable);
            placeable = null;
            prefabToPlace = null;
        }
    }
}
