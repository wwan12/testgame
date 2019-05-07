using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class map_2d : MonoBehaviour
{

    public Tilemap runMap;//引用的Tilemap
    public Tilemap wallMap; 
    /// <summary>
    /// 地板Tile
    /// </summary>
    public Tile baseTile;
    /// <summary>
    /// 资源tile
    /// </summary>
    public Tile[] resourcesTiles;
    /// <summary>
    /// 墙tile
    /// </summary>
    public Tile wallTile;

    public Vector3Int startTile;
    Tile[] arrTiles;//生成的Tile数组

    int[,] map;
    [Range(0, 100)] //这行代码用来在面板上显示一个滑动条
    public int probability;
//地图的长和高
public int width=8;
    public int height=8;
    //种子
    public string seed;
    //是否要使用随机种子
    public bool useRandomSeed;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //空白地方创造墙体
    void DrawTile(Vector3 vector, Tile tile) {
       // Vector3 mousePosition = Input.mousePosition;
       // Vector3 wordPosition = Camera.main.ScreenToWorldPoint(vector);
        Vector3Int cellPosition = runMap.WorldToCell(vector);
        //tilemap.SetTile(cellPosition, gameUI.GetSelectColor().colorData.mTile);
        //TileBase tb = tilemap.GetTile(cellPosition);
        //if (tb != null)
        //{
        //    return;
        //}
        //tb.hideFlags = HideFlags.None;
        //Debug.Log("鼠标坐标" + mousePosition + "世界" + wordPosition + "cell" + cellPosition + "tb" + tb.name);
        //格子填充
        runMap.SetTile(cellPosition, tile);
        //tilemap.RefreshAllTiles();

    }
    //销毁墙体
    void RemoveTile(Vector3 vector) {
       // Vector3 mousePosition = Input.mousePosition;
        Vector3 wordPosition = Camera.main.ScreenToWorldPoint(vector);
        Vector3Int cellPosition = runMap.WorldToCell(wordPosition);
        //tilemap.SetTile(cellPosition, gameUI.GetSelectColor().colorData.mTile);
        TileBase tb = runMap.GetTile(cellPosition);
        if (tb == null)
        {
            return;
        }
        //tb.hideFlags = HideFlags.None;
        Debug.Log( "世界" + wordPosition + "cell" + cellPosition + "tb" + tb.name);
        //某个地方设置为空，就是把那个地方小格子销毁了
        runMap.SetTile(cellPosition, null);
        //tilemap.RefreshAllTiles();
    }


    /// <summary>
    /// 地图生成
    /// </summary>
    /// <returns></returns>
    IEnumerator InitData()
    {
        //大地图宽高
        int levelW = 10;
        int levelH = 10;

        int colorCount = 6;
        arrTiles = new Tile[colorCount];
        for (int i = 0; i < colorCount; i++)
        {
            //想做生命墙，需要自己做个数据层，对应索引id就行
            arrTiles[i] = ScriptableObject.CreateInstance<Tile>();//创建Tile，注意，要使用这种方式
          //  arrTiles[i].sprite = baseTile.sprite;
            arrTiles[i].color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1);
        }
        for (int i = 0; i < levelH; i++)
        {//这里就是设置每个Tile的信息了
            for (int j = 0; j < levelW; j++)
            {
                runMap.SetTile(new Vector3Int(j, i, 0), arrTiles[UnityEngine.Random.Range(0, arrTiles.Length)]);
            }
            yield return null;
        }

        while (true)
        {
            yield return new WaitForSeconds(2);
            // int colorIdx = Random.Range(0, colorCount);//前面这个是随机将某个块的颜色改变，然后让Tilemap更新，主要用来更新Tile的变化
            // arrTiles[colorIdx].color = new Color(Random.Range(0f, 1f), Random.Range(0f,1f), Random.Range(0f, 1f), 1);
            // tilemap.RefreshAllTiles();

            Color c = runMap.color;//这里是改变Tilemap的颜色，尝试是否可以整体变色
            c.a -= Time.deltaTime;
            runMap.color = c;
        }
    }

    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();    //随机生成地图

        for (int i = 0; i < 2; i++)
        {
            SmoothMap();
        }
        DrawMap();
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
            seed = DateTime.Now.ToString();

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (i == 0 || i == width - 1 || j == 0 || j == height - 1)  //边缘是墙
                    map[i, j] = 1;
                else
                    map[i, j] = (pseudoRandom.Next(0, 100) < probability) ? 1 : 0;  //1是墙，0是空地
            }
        }
       

    }


    void DrawMap()
    {
        if (map != null)
        {
            map[startTile.x, startTile.y] = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    EventManage.Instance.LogWrap(map[i, j].ToString());
                    if (map[i, j] == 0)
                    {
                        runMap.SetTile(new Vector3Int(j, i, 0),baseTile );
                    }
                    else
                    {
                        wallMap.SetTile(new Vector3Int(j, i, 0), wallTile);
                    }
                  
                }
            }
        }
    }


//**去噪点
    int GetSurroundingWalls(int posX, int posY)
    {
        int wallCount = 0;

        for (int i = posX - 1; i <= posX + 1; i++)
        {
            for (int j = posY - 1; j <= posY + 1; j++)
            {
                if (i >= 0 && i < width && j >= 0 && j < height)
                {
                    if (i != posX || j != posY)
                        wallCount += map[i, j];
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }
    void SmoothMap()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int surroundingTiles = GetSurroundingWalls(i, j);

                if (surroundingTiles > 4)
                    map[i, j] = 1;
                else if (surroundingTiles < 4)
                    map[i, j] = 0;
            }
        }
    }
}
