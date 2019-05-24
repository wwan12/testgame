using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
        gameObject.AddComponent<Rigidbody2D>().bodyType=RigidbodyType2D.Static;
        gameObject.AddComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Action() {
        menu= Resources.Load<GameObject>("Warehouse/Builds/Shuttle/LoadMenu.prefab");
        menu= GameObject.Instantiate<GameObject>(menu);
        menu.GetComponent<Button>().onClick.AddListener(ToEventPointScene);
    }
    private void Progress(object o,int progress) {

    }

    void ToEventPointScene() {
        AppManage.Instance.SaveParameter("ToMapName", ToMapName);
        AppManage.Instance.LoadSceneCallBack += Progress;
        AppManage.Instance.StartLoadScene(this, SceneManager.LoadSceneAsync("EventPointScene", LoadSceneMode.Additive));
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
