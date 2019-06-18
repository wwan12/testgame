using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildWarehouse : MonoBehaviour
{
    GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        gameObject.AddComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Action()
    {
        menu = Resources.Load<GameObject>("Warehouse/Builds/Warehouse/Warehouse.prefab");
        menu = GameObject.Instantiate<GameObject>(menu);
       
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
}
