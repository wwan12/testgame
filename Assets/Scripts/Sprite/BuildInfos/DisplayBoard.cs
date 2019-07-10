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

        if (BuildMenu.board!=null)
        {
            Destroy(BuildMenu.board);
            BuildMenu.board = null;
        }
        GameObject board= Resources.Load<GameObject>("prefabs/UI/BuildDisplayBoard");      
        board= GameObject.Instantiate<GameObject>(board);
        board.GetComponent<DisplayBoard>().control = control;
        board.GetComponentInChildren<Text>().text = explain;
        board.transform.SetParent(AppManage.Instance.HUD.transform,false);
        board.transform.SetAsFirstSibling();
        BuildMenu.board = board;
       // board.transform.position = Camera.main.WorldToScreenPoint(position);转到被点击建筑
       
    }

    public void Close()
    {       
        Destroy(gameObject);
     
    }

    public void Open() {
        // control.TryConnect();
        avt = !avt;
        if (avt)
        {
            control.available = true;
            control.OnAvailable();
        }
        else
        {
            control.available = false;
            control.OnNotAvailable();
        }      
    }

    public void Dismantle()
    {
    
        control.RemoveBuild();
    }



}
