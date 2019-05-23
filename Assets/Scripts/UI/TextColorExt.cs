using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextColorExt : BaseMeshEffect
{
    private enum Direction
    {
        Vertical,
        Horizontal,
    }
    [SerializeField]
    private Direction mDirection = Direction.Vertical;
    [SerializeField]
    private Color color = Color.white;
    [SerializeField]
    private bool isMid = true;
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
            return;
        if (vh.currentVertCount == 0)
            return;
        if (isMid)
        {
            List<UIVertex> vlist = new List<UIVertex>();
            for (int i = 0; i < vh.currentVertCount; i += 4)
            {
                UIVertex ver0 = new UIVertex();
                UIVertex ver1 = new UIVertex();
                UIVertex ver3 = new UIVertex();
                UIVertex ver4 = new UIVertex();
                vh.PopulateUIVertex(ref ver0, i + 0);
                vh.PopulateUIVertex(ref ver1, i + 1);
                vh.PopulateUIVertex(ref ver3, i + 2);
                vh.PopulateUIVertex(ref ver4, i + 3);

                UIVertex ver2 = new UIVertex();
                ver2.position = (ver1.position + ver3.position) / 2;
                ver2.uv0 = (ver1.uv0 + ver3.uv0) / 2;
                ver2.color = color;
                ver2.normal = ver0.normal;
                ver2.tangent = ver0.tangent;

                UIVertex ver5 = new UIVertex();
                ver5.position = (ver0.position + ver4.position) / 2;
                ver5.uv0 = (ver0.uv0 + ver4.uv0) / 2;
                ver5.color = color;
                ver5.normal = ver0.normal;
                ver5.tangent = ver0.tangent;

                vlist.Add(ver0);
                vlist.Add(ver1);
                vlist.Add(ver2);
                vlist.Add(ver3);
                vlist.Add(ver4);
                vlist.Add(ver5);
            }

            List<int> tlist = new List<int>();
            for (int i = 0; i < vlist.Count; i += 6)
            {
                tlist.Add(i + 0);
                tlist.Add(i + 1);
                tlist.Add(i + 2);

                tlist.Add(i + 0);
                tlist.Add(i + 2);
                tlist.Add(i + 5);

                tlist.Add(i + 2);
                tlist.Add(i + 3);
                tlist.Add(i + 4);

                tlist.Add(i + 2);
                tlist.Add(i + 4);
                tlist.Add(i + 5);
            }
           vh.Clear();
            vh.AddUIVertexStream(vlist, tlist);
        }
        else
        {
            List<UIVertex> vlist = new List<UIVertex>();
            vh.GetUIVertexStream(vlist);
            if (vlist == null || vlist.Count == 0)
                return;

            Color startColr = vlist[0].color;

            float leftX = vlist[0].position.x;
            float rightX = vlist[0].position.x;

            float bottomY = vlist[0].position.y;
            float topY = vlist[0].position.y;

            for (int i = 1; i < vlist.Count; i++)
            {
                float y = vlist[i].position.y;
                if (y > topY)
                {
                    topY = y;
                }
                else if (y < bottomY)
                {
                    bottomY = y;
                }
                float x = vlist[i].position.x;
                if (x > rightX)
                {
                    rightX = x;
                }
                else if (x < leftX)
                {
                    leftX = x;
                }
            }

            for (int i = 0; i < vlist.Count; i++)
            {
                UIVertex ver = vlist[i];
                float t = 0f;
                if (mDirection == Direction.Vertical)
                {
                    t = (ver.position.y - bottomY) / (topY - bottomY);
                }
                else
                {
                    t = (ver.position.x - leftX) / (rightX - leftX);
                }
                ver.color = Color.Lerp(startColr, color, t);
                vlist[i] = ver;
            }

            vh.Clear();
            vh.AddUIVertexTriangleStream(vlist);
        }
    }
}

