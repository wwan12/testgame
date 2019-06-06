using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHelper
{
    public  Transform canvas;
    public float lineWidth = 4f;
    public string name="def";

    public void DrawPaths(List<Vector3> paths)
    {
        for (int i = 0; i < paths.Count - 1; i++)
        {
            Image lineImage = GameObject.Instantiate(Resources.Load<GameObject>("prefabs/Node/LineImage")).GetComponent<Image>();
            lineImage.name = name + "Line";
            if (paths[i].y - paths[i + 1].y == 0)//判断画的是横线还是竖线
            {
                lineImage.transform.position = new Vector2(paths[i].x - (paths[i].x - paths[i + 1].x) / 2, paths[i].y);
                lineImage.rectTransform.sizeDelta = new Vector2(Mathf.Abs(paths[i].x - paths[i + 1].x) + lineWidth, lineWidth);
            }
            else if (paths[i].x - paths[i + 1].x == 0)
            {
                lineImage.transform.position = new Vector2(paths[i].x, paths[i].y - (paths[i].y - paths[i + 1].y) / 2);
                lineImage.rectTransform.sizeDelta = new Vector2(lineWidth, Mathf.Abs(paths[i].y - paths[i + 1].y));
            }
            else//斜线 todo 旋转后锯齿
            {
                lineImage.transform.position = new Vector2(paths[i].x - (paths[i].x - paths[i + 1].x) / 2, paths[i].y - (paths[i].y - paths[i + 1].y) / 2);
                lineImage.rectTransform.sizeDelta = new Vector2(lineWidth, Vector2.Distance(paths[i], paths[i + 1]));//计算起点终点两点距离);
                float angle = Vector3.Angle(paths[i], paths[i + 1]);
                lineImage.transform.localRotation = Quaternion.AngleAxis(-(angle + angle), Vector3.forward);
            }

            lineImage.transform.SetParent(canvas, false);
            lineImage.transform.SetAsFirstSibling();
            //line= GameObject.Instantiate<GameObject>(line);
        }
    }
}
