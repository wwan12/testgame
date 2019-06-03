using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile 
{
    public bool isWall;
    public int Index;
    public bool isHasRes;
    public int resIndex;
    public int resAllowance;
    public int posX;
    public int posY;
    public int posZ;

    public Vector3 pos;
    public MapTile parentNode;

    public int costG;
    public int costH;

    

    public int CostF
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
