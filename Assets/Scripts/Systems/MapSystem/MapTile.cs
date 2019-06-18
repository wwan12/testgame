using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile 
{
    /// <summary>
    /// 墙？
    /// </summary>
    public bool isWall;
    /// <summary>
    /// 地块显示序号
    /// </summary>
    public int Index;
    /// <summary>
    /// 资源？
    /// </summary>
    public bool isHasRes;
    /// <summary>
    /// 资源序号
    /// </summary>
    public int resIndex;
    /// <summary>
    /// 资源量
    /// </summary>
    public int resAllowance;
    /// <summary>
    /// 资源名称
    /// </summary>
    public string resName;
    public int posX;
    public int posY;
    public int posZ;

    public Vector3 pos;
    public MapTile parentNode;

    public float costG;
    public float costH;

    

    public float CostF
    {
        get { return costG + costH; }
    }

    public MapTile(bool _isWall, Vector3 _pos, int _z, int _x)
    {
        this.isWall = _isWall;
        this.pos = _pos;
        this.posX = _x;
        this.posZ = _z;
    }
    public MapTile(int _x, int _y) {
        this.posX = _x;
        this.posY = _y;
    }
}
