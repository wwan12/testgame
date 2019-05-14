using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

//todo 生成资源地图,摄像机视野到达边界时自动生成，与地块交互，与资源地块交互,兴趣点
public class map_2d : MonoBehaviour
{
    [Tooltip(" 道路地图")]
    public Tilemap runMap;//引用的Tilemap
    [Tooltip(" 墙地图")]
    public Tilemap wallMap;
    [Tooltip("资源地图")]
    public Tilemap resMap;//
 
    [Tooltip(" 地板Tile")]
    public Tile[] baseTile;
    [Tooltip("资源tile")]
    public Tile[] resourcesTiles;

    [Tooltip("墙tile")]
    public Tile wallTile;

    public Vector3Int startTile;
 
    int[,][] map;//二维地图 x纵列坐标，y横列坐标,(0道路，1墙),(展示tile序号),(结束符|分隔符,)
    int[,][] resourcesMap;//资源地图,x纵列坐标,y横列坐标,(0无资源，1有资源)，(资源类型),(结束符|)

    [Range(0, 100)]
    public int probability;
    /// <summary>
    /// 资源密度
    /// </summary>
    [Range(0, 100)] 
    public int resourceDensity;
    //地图的长和高
    public int width=8;
    public int height=8;
    //种子
    public string seed;
    //是否要使用随机种子
    public bool useRandomSeed;
    [Tooltip("是否使用覆盖式资源")]
    public bool isCover;
    //Interest point

    // Start is called before the first frame update
    void Start()
    {
        map = new int[width, height][];
        resourcesMap = new int[width, height][];
        if (AppManage.Instance.saveData.isNew)
        {
            GenerateMap();
        }
        else {
            ReadMap(AppManage.Instance.saveData.mapData);
        }
       
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateMap()
    {
        RandomFillMap();    //随机生成地图
        RandomFillResMap();
        for (int i = 0; i < 2; i++)
        {
            SmoothMap();
            SmoothResMap();
        }
        DrawMap();
    }
    //用墙和道路填充地图
    void RandomFillMap()
    {
        if (useRandomSeed)
            seed = DateTime.Now.ToString();

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (i == 0 || i == width - 1 || j == 0 || j == height - 1)
                {
                    //边缘是墙
                    map[i, j] = new int[2];
                    map[i, j][0] = 1;
                }
                else {
                    map[i, j] = new int[2];
                    map[i, j][0] = (pseudoRandom.Next(0, 100) < probability) ? 1 : 0;  //1是墙，0是空地
                }
            }
        }
    }

    void RandomFillResMap() {
        if (useRandomSeed)
            seed = DateTime.Now.ToString();

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (i == 0 || i == width - 1 || j == 0 || j == height - 1)
                {
                    //边缘是不生成资源
                    resourcesMap[i, j] = new int[2];
                    resourcesMap[i, j][0] = 0;
                }
                else
                {
                    resourcesMap[i, j] = new int[2];
                    resourcesMap[i, j][0] = (pseudoRandom.Next(0, 100) < resourceDensity) ? 1 : 0; 
                }
            }
        }
    }


    void DrawMap()
    {
        if (map != null)
        {
            map[startTile.x, startTile.y][0] = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (map[i, j][0] == 0)
                    {
                        runMap.SetTile(new Vector3Int(j-width/2, i-height/2, 0), baseTile[map[i,j][1]] );
                    }
                    else
                    {
                        wallMap.SetTile(new Vector3Int(j - width / 2, i - height / 2, 0), wallTile);
                    }          
                }
            }
        }
    }

    public void CreateMap() {


    }

    //储存地图
    public void SaveMap() {
        StringBuilder saveData = new StringBuilder();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                saveData.Append(i);
                saveData.Append(",");
                saveData.Append(j);
                saveData.Append(",");
                for (int c = 0; c < map[i, j].Length; c++)
                {
                    saveData.Append(map[i,j][c]);
                    if (!(c== map[i, j].Length-1))
                    {
                        saveData.Append(",");
                    }
                }
                saveData.Append("|");
            }
        }
        AppManage.Instance.saveData.mapData = saveData.ToString();
    }
    //读取地图
    public void ReadMap(string save) {
        string[] tileDatas= save.Split('|');
        for (int i = 0; i < tileDatas.Length; i++)
        {
            string[] tileData= tileDatas[i].Split(',');
            for (int j = 2; j < tileData.Length; j++)
            {  
               map[int.Parse(tileData[0]), int.Parse(tileData[1])][j]=int.Parse(tileData[j]);           
            }
        }
        DrawMap();
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
                        wallCount += map[i, j][0];
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
                    map[i, j][0] = 1;
                else if (surroundingTiles < 4)
                    map[i, j][0] = 0;
            }
        }
    }
    void SmoothResMap()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int surroundingTiles = GetSurroundingWalls(i, j);

                if (surroundingTiles > 4)
                    resourcesMap[i, j][0] = 1;
                else if (surroundingTiles < 4)
                    resourcesMap[i, j][0] = 0;
            }
        }
    }
    //空白地方创造墙体
    void DrawTile(Vector3 vector, Tile tile)
    {
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
    void RemoveTile(Vector3 vector)
    {
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
        Debug.Log("世界" + wordPosition + "cell" + cellPosition + "tb" + tb.name);
        //某个地方设置为空，就是把那个地方小格子销毁了
        runMap.SetTile(cellPosition, null);
        //tilemap.RefreshAllTiles();
    }
    /// <summary>
    /// 地图生成
    /// </summary>
    /// <returns></returns>
    //IEnumerator InitData()
    //{
    //    //大地图宽高
    //    int levelW = 10;
    //    int levelH = 10;

    //    int colorCount = 6;
    //    arrTiles = new Tile[colorCount];
    //    for (int i = 0; i < colorCount; i++)
    //    {
    //        //想做生命墙，需要自己做个数据层，对应索引id就行
    //        arrTiles[i] = ScriptableObject.CreateInstance<Tile>();//创建Tile，注意，要使用这种方式
    //                                                              //  arrTiles[i].sprite = baseTile.sprite;
    //        arrTiles[i].color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1);
    //    }
    //    for (int i = 0; i < levelH; i++)
    //    {//这里就是设置每个Tile的信息了
    //        for (int j = 0; j < levelW; j++)
    //        {
    //            runMap.SetTile(new Vector3Int(j, i, 0), arrTiles[UnityEngine.Random.Range(0, arrTiles.Length)]);
    //        }
    //        yield return null;
    //    }

    //    while (true)
    //    {
    //        yield return new WaitForSeconds(2);
    //        // int colorIdx = Random.Range(0, colorCount);//前面这个是随机将某个块的颜色改变，然后让Tilemap更新，主要用来更新Tile的变化
    //        // arrTiles[colorIdx].color = new Color(Random.Range(0f, 1f), Random.Range(0f,1f), Random.Range(0f, 1f), 1);
    //        // tilemap.RefreshAllTiles();

    //        Color c = runMap.color;//这里是改变Tilemap的颜色，尝试是否可以整体变色
    //        c.a -= Time.deltaTime;
    //        runMap.color = c;
    //    }
    //}
}
