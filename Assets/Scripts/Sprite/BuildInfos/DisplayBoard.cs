using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBoard : MonoBehaviour
{
    BuildControl control;
    bool avt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Show(BuildControl control,Vector3 position, string explain) {
        if (BuildMenu.CacheBoard != null)
        {
            Destroy(BuildMenu.CacheBoard);
            BuildMenu.CacheBoard = null;
        }
        GameObject board= Resources.Load<GameObject>("prefabs/UI/BuildDisplayBoard");
        board.GetComponent<DisplayBoard>().control = control;
        board.GetComponentInChildren<Text>().text = explain;
        board= GameObject.Instantiate<GameObject>(board);
        board.transform.SetParent(AppManage.Instance.HUD.transform,false);
       // board.transform.position = Camera.main.WorldToScreenPoint(position);转到被点击建筑
        BuildMenu.CacheBoard = board;
    }

    public void Close()
    {       
        Destroy(BuildMenu.CacheBoard);
        BuildMenu.CacheBoard = null;
    }

    public void Open() {
        // control.TryConnect();
        avt = !avt;
        if (avt)
        {
            control.OnAvailable();
        }
        else
        {
            control.OnNotAvailable();
        }      
    }

    public void Dismantle()
    {
        control.RemoveBuild();
    }



}
