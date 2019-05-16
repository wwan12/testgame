using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 穿梭机
/// </summary>
public class Shuttle : MonoBehaviour
{
    [Tooltip("要去地图的名称")]
    public string ToMapName;
    GameObject menu;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Action() {
       menu= Resources.Load<GameObject>("Warehouse/Builds/Shuttle/LoadMenu.prefab");
        menu= GameObject.Instantiate<GameObject>(menu);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(1))
        {
            //todo 打开装载页，可以选择负载物
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManage>().InOperationRange(gameObject.transform.position))
            {
                Action();
            }
          
        }
    }

  
    private void OnMouseExit()
    {

    }
}
