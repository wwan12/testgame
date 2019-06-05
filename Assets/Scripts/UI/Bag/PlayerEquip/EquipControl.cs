using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipControl : MonoBehaviour
{
    public RectTransform[] connects;
    public string[] tags;
    public float lineWidth;
    private Image lineImage;
    private LatticeController lattice;
    private RectTransform canvas;

    // Start is called before the first frame update
    void Start()
    {
       lattice= gameObject.AddComponent<LatticeController>();
        lattice.tagOfSupport = tags;
       lattice.itemInfoPanel= GameObject.FindObjectOfType<BagManage>().itemInfoPanel;
        canvas = GameObject.FindObjectOfType<BagManage>().GetComponent<RectTransform>();
        lattice.canvas = canvas;
        lattice.UseThisItemCallBack += UseEquip;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UseEquip(object obj,ItemInfo info)
    {

    }

    void NextLinksPaths()
    {
        foreach (var connect in connects)
        {
            float startX = gameObject.transform.position.x;
            float startY = gameObject.transform.position.y;
            float endX = connect.gameObject.transform.position.x;
            float endY = connect.gameObject.transform.position.y;
            float angle = Vector3.Angle(gameObject.transform.position, connect.gameObject.transform.position);
            if (0<angle&&angle<90)//在右上
            {

            }
            if (90 < angle && angle < 180)
            {

            }
            if (180 < angle && angle < 270)
            {

            }
            if (270 < angle && angle < 360)
            {

            }

            List<Vector3> paths = new List<Vector3>();
            if (startX == endX)
            {
                paths.Add(new Vector3(startX, startY, 0));//起点
                paths.Add(new Vector3(endX, endY, 0));//结束点
            }
            else
            {

                paths.Add(new Vector3(startX, -Screen.height + startY, 0));//起点
                paths.Add(new Vector3(startX, -Screen.height + (startY + endY) / 2, 0));//中继点a
                paths.Add(new Vector3(endX, -Screen.height + (startY + endY) / 2, 0));//中继点b
                paths.Add(new Vector3(endX, -Screen.height + endY, 0));//结束点
            }
            DrawPaths(paths,angle);
        }

    }


    void DrawPaths(List<Vector3> paths,float angle=0)
    {
        for (int i = 0; i < paths.Count - 1; i++)
        {
            lineImage = GameObject.Instantiate(Resources.Load<GameObject>("prefabs/Node/LineImage")).GetComponent<Image>();
            if (paths[i].y - paths[i + 1].y == 0)//判断画的是横线还是竖线
            {
                lineImage.transform.position = new Vector2(paths[i].x - (paths[i].x - paths[i + 1].x) / 2, paths[i].y);
                lineImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Abs(paths[i].x - paths[i + 1].x) + lineWidth, lineWidth);
            }
            else if(paths[i].x - paths[i + 1].x == 0)
            {
                lineImage.transform.position = new Vector2(paths[i].x, paths[i].y - (paths[i].y - paths[i + 1].y) / 2);
                lineImage.GetComponent<RectTransform>().sizeDelta = new Vector2(lineWidth, Mathf.Abs(paths[i].y - paths[i + 1].y));
            }
            else//斜线
            {
                lineImage.transform.position = new Vector2(paths[i].x - (paths[i].x - paths[i + 1].x) / 2, paths[i].y - (paths[i].y - paths[i + 1].y) / 2);
                lineImage.GetComponent<RectTransform>().sizeDelta = new Vector2(lineWidth, Mathf.Sin(angle)* (Mathf.Abs(paths[i].x - paths[i + 1].x) + lineWidth));
            }
            lineImage.transform.SetParent(canvas, false);
            lineImage.transform.SetAsFirstSibling();
            //line= GameObject.Instantiate<GameObject>(line);
        }

    }

}
