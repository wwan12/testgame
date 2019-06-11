using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

//todo 生成资源地图,摄像机视野到达边界时自动生成，与地块交互，与资源地块交互,兴趣点
public class MapManage : MonoBehaviour
{
    [Header("生成资源设置")]
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
    [Tooltip("视野边际的碰撞体")]
    public GameObject boundary;
    /// <summary>
    /// 二维地图 [x纵列坐标，y横列坐标]
    /// (0道路，1墙),(展示tile序号),(0无资源，1有资源)，(资源类型序号),(资源余量)(结束符|分隔符,)
    /// </summary>
    public MapTile[,] map { get; private set; }
    //int[,][] resourcesMap;//资源地图,[x纵列坐标,y横列坐标](0无资源，1有资源)，(资源类型序号),(资源余量)(结束符|)
    readonly int saveMapLength = 5;//存档长度
    //readonly int saveResLength = 3;//存档长度
    readonly char mapEnd = '|';
    //readonly char middleEnd = '>';
    [Header("生成参数设置")]
    [Tooltip("障碍物密度")]
    [Range(0, 100)]
    public int probability;
    [Tooltip("资源密度")]
    [Range(0, 60)]
    public int resourceDensity;
    [Tooltip("资源丰度")]
    [Range(0, 100)]
    public int resourceAbundance;
    [Tooltip("宽")]
    public int width = 8;
    [Tooltip("高")]
    public int height = 8;
    [Tooltip("种子")]
    public int seed;
    [Tooltip("是否要使用随机种子")]
    public bool useRandomSeed;
    [Tooltip("是否使用覆盖式资源")]
    public bool isCover;
    [Tooltip("是否用墙包裹地图")]
    public bool isSurround;
    [Tooltip("地图每次扩大的长宽")]
    public int ext=32;
    [Header("小地图设置")]
    [Tooltip("小地图，设置minimap图层")]
    public Tilemap miniMap;
    [Tooltip("代表墙的tile")]
    public Tile miniWallTile;
    [Tooltip("代表道路的tile")]
    public Tile miniRunTile;

    //private bool isExhausted;
    private System.Random pseudoRandom;
    private System.Random pseudoRandomRes;
    private int oldWidth;
    private int oldHeight;
    [HideInInspector]
    public List<BaseCharacter> characters;
    //private bool hasPlayer;
    //private GameObject player;
    //Interest point


    // Start is called before the first frame update
    void Start()
    {
        map = new MapTile[width, height];
      
        // player = GameObject.FindGameObjectWithTag("Player");
#if  UNITY_EDITOR
        CreateMap();
#endif

        //  ChangeTile();
    }

    // Update is called once per frame
    void Update()
    {

        //for (int i = 0; i < width; i++)//监控资源余量为0时销毁对应资源地块
        //{
        //    for (int j = 0; j < height; j++)
        //    {
        //        if (map[i, j].isHasRes && map[i, j].resAllowance == 0)
        //        {
        //            map[i, j].isHasRes = false;
        //            resMap.SetTile(new Vector3Int(j - width / 2, i - height / 2, 0), null);
        //        }
        //    }
        //}


    }

    void GenerateMap()
    {
        RandomFillMap();    //随机生成地图
                            // RandomFillResMap();
        for (int i = 0; i < 2; i++)
        {
            SmoothMap(map);
           // SmoothMap(map, 1);
        }
        DrawMap();
        //DrawMiniMap();
    }
    /// <summary>
    /// 第一次填充地图
    /// </summary>
    void RandomFillMap()
    {
        if (useRandomSeed)
            seed = DateTime.Now.ToString().GetHashCode();

        pseudoRandom = new System.Random(seed);
        pseudoRandomRes = new System.Random(seed + 99);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                map[i, j] = new MapTile(i, j);
                if (isSurround && (i == 0 || i == width - 1 || j == 0 || j == height - 1))
                {
                    //边缘是墙
                    map[i, j].isWall = true;
                }
                else {

                    if (width / 2 - 2 < i && i < width / 2 + 2 && height / 2 - 2 < j && j < height / 2 + 2)//中间空出3*3的格子
                    {
                        map[i, j].isWall = false;
                    }
                    else
                    {
                        map[i, j].isWall = (pseudoRandom.Next(0, 100) < probability);  //1是墙，0是空地
                        map[i, j].isHasRes = (pseudoRandomRes.Next(0, 100) < resourceDensity);
                        map[i, j].resAllowance = (int)Rand(resourceAbundance, resourceAbundance / 4);//第一次生成
                    }
                }
            }
        }
    }



    void DrawMap()
    {
        if (map != null)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (!map[i, j].isWall)//画出道路和墙,从0，0开始画
                    {
                        runMap.SetTile(new Vector3Int(j - width / 2, i - height / 2, 0), baseTile[map[i, j].Index]);
                        if (map[i, j].isHasRes)
                        {
                            resMap.SetTile(new Vector3Int(j - width / 2, i - height / 2, 0), resourcesTiles[map[i, j].resIndex]);
                        }
                        miniMap.SetTile(new Vector3Int(j - width / 2, i - height / 2, 0), miniRunTile);
                    }
                    else
                    {
                        wallMap.SetTile(new Vector3Int(j - width / 2, i - height / 2, 0), wallTile);
                        miniMap.SetTile(new Vector3Int(j - width / 2, i - height / 2, 0), miniWallTile);
                    }
                }
            }
        }
        BakeMap();
    }
    [Obsolete]
    void DrawMiniMap()
    {
        if (miniMap == null)
        {
            return;
        }
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (!map[i, j].isWall)//画出道路和墙
                {
                    miniMap.SetTile(new Vector3Int(j - width / 2, i - height / 2, 0), miniRunTile);
                }
                else
                {
                    miniMap.SetTile(new Vector3Int(j - width / 2, i - height / 2, 0), miniWallTile);
                }
            }
        }
    }

    void ExtDrawMap() {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (ext < i && i < width - ext && ext < j && j < height - ext)//已加载的地图数据不变
                {

                }
                else
                {
                    if (!map[i, j].isWall)//画出道路和墙,从0，0开始画
                    {
                        runMap.SetTile(new Vector3Int(j - width / 2, i - height / 2, 0), baseTile[map[i, j].Index]);
                        if (map[i, j].isHasRes)
                        {
                            resMap.SetTile(new Vector3Int(j - width / 2, i - height / 2, 0), resourcesTiles[map[i, j].resIndex]);
                        }
                        miniMap.SetTile(new Vector3Int(j - width / 2, i - height / 2, 0), miniRunTile);
                    }
                    else
                    {
                        wallMap.SetTile(new Vector3Int(j - width / 2, i - height / 2, 0), wallTile);
                        miniMap.SetTile(new Vector3Int(j - width / 2, i - height / 2, 0), miniWallTile);
                    }
                }

            }
        }
        
    }

    void BakeMap() {
      //  runMap.GetComponent<NavMeshSurface>().BuildNavMesh();

    }
    /// <summary>
    /// 创建新地图
    /// </summary>
    public void CreateMap() {
        ChangeBoundary();
        GenerateMap();
    }
    /// <summary>
    /// 创建新地图
    /// </summary>
    public void CreateMap(int seed)
    {
        this.seed = seed;
        useRandomSeed = false;
        CreateMap();
    }
    /// <summary>
    /// 修改边缘碰撞体大小
    /// </summary>
    void ChangeBoundary()
    {
        float deviation = width / 10;
        boundary.GetComponent<PolygonCollider2D>().points = new Vector2[] { new Vector2(-width / 2 + deviation, height / 2 - deviation), new Vector2(-width / 2 + deviation, -height / 2 + deviation), new Vector2(width / 2 - deviation, -height / 2 + deviation), new Vector2(width / 2 - deviation, height / 2 - deviation) };
    }
    /// <summary>
    /// 使用设置大小扩充地图
    /// </summary>
    public void ExpandMap() {
        StartCoroutine(ExpandMap(ext));
    }
   

    /// <summary>
    /// 扩充地图
    /// </summary>
    /// <param name="extra"></param>
    IEnumerator ExpandMap(int extra)
    {
        oldWidth = width;
        oldHeight = height;
        width += 2*extra;
        height += 2*extra;
        MapTile[,] expand = new MapTile[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
               
                // expand[i, j] = new int[saveMapLength];
                if (extra < i && i < width - extra && extra < j && j < height - extra)//已加载的地图数据不变
                {
                    expand[i, j] = null;
                }
                else
                {
                    expand[i, j] = new MapTile(i, j);
                    expand[i, j].isWall = (pseudoRandom.Next(0, 100) < probability);  //1是墙，0是空地
                    expand[i, j].isHasRes = (pseudoRandomRes.Next(0, 100) < resourceDensity);
                    expand[i, j].resAllowance = (int)Rand(resourceAbundance, resourceAbundance / 4);//第一次生成
                }

            }
        }
        for (int i = 0; i < 2; i++)
        {
            SmoothMap(expand);
           // SmoothMap(expand);
        }
        for (int i = 0; i < width -2* extra; i++)
        {
            for (int j = 0; j < height - 2*extra; j++)
            {
                expand[i + extra, j + extra] = map[i, j];
            }
        }
        yield return new WaitForEndOfFrame();
        map = expand;
        ExtDrawMap();
        yield return new WaitForEndOfFrame();
        //DrawMiniMap();
        ChangeBoundary();
        yield return null;
    }


    /// <summary>
    /// 储存地图
    /// </summary>
    public string SaveMap() {
        StringBuilder saveMapData = new StringBuilder();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                saveMapData.Append(i);
                saveMapData.Append(",");
                saveMapData.Append(j);
                saveMapData.Append(",");
                saveMapData.Append(map[i, j].isWall ? "1" : "0");
                saveMapData.Append(",");
                saveMapData.Append(map[i, j].Index.ToString());
                saveMapData.Append(",");
                saveMapData.Append(map[i, j].isHasRes ? "1" : "0");
                saveMapData.Append(",");
                saveMapData.Append(map[i, j].resIndex.ToString());
                saveMapData.Append(",");
                saveMapData.Append(map[i, j].resAllowance.ToString());
                saveMapData.Append(mapEnd);
            }
        }
        return saveMapData.ToString();
    }

    /// <summary>
    ///  读取地图
    /// </summary>
    /// <param name="save"></param>
    public void ReadMap(string save) {
        string[] tileDatas= save.Split(mapEnd);
        for (int i = 0; i < tileDatas.Length; i++)
        {
            string[] tileData = tileDatas[i].Split(',');
            int x = int.Parse(tileData[0]);
            int y = int.Parse(tileData[1]);
            //  map[x, y] = new MapTile(x,y);
            map[x, y] = NumsToTile(tileData, x, y);
        }
        DrawMap();
       // DrawMiniMap();
    }

    MapTile NumsToTile(string[] data,int x,int y)
    {
        MapTile tile = new MapTile(x, y)
        {
            isWall = (int.Parse(data[2]) == 1),
            Index = int.Parse(data[3]),
            isHasRes = (int.Parse(data[4]) == 1),
            resIndex = int.Parse(data[5]),
            resAllowance = int.Parse(data[6]),
        };
        return tile;
    }


    /// <summary>
    /// 去噪点
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    int GetSurroundingWalls(int posX, int posY, MapTile[,] data)
    {
        int wallCount = 0;

        for (int i = posX - 1; i <= posX + 1; i++)
        {
            for (int j = posY - 1; j <= posY + 1; j++)
            {
                if (i >= 0 && i < width && j >= 0 && j < height)
                {
                    if (i != posX || j != posY) {
                        if (data[i,j]!=null)
                        {
                            wallCount += data[i, j].isWall ? 1 : 0;
                        }
                        else
                        {
                            wallCount++;
                        }                 
                    }
                }
                //else
                //{
                //    wallCount++;
                //}
            }
        }

        return wallCount;
    }
    /// <summary>
    /// 将随机生成的数据集群化
    /// </summary>
    void SmoothMap(MapTile[,] data)
    {
        //data.GetLength(0);
        //data.GetLength(1);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (data[i, j] == null)
                {
                    continue;
                }
                int surroundingTiles = 0;
                surroundingTiles = GetSurroundingWalls(i, j, data);
                if (surroundingTiles > 4)
                    data[i, j].isWall = true;//变成wall
                else if (surroundingTiles < 4)
                    data[i, j].isWall = false;
            }
        }
    }

    Vector3Int GetClickPosition(Tilemap map)
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 wordPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int cellPosition = map.WorldToCell(wordPosition);
        return cellPosition;

    }

    /// <summary>
    /// 随机产生一个符合正态分布的数 u均数，d为方差
    /// </summary>
    /// <param name="u"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public double Rand(double u, double d)
    {
        double u1, u2, z, x;
        if (d <= 0)
        {
            return u;
        }
        u1 = new System.Random().NextDouble();
        u2 = new System.Random().NextDouble();
        z = Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);
        x = u + d * z;
        return x;
    }

  

    public void ChangeTile()
    {
        
        runMap.SetColor(new Vector3Int(width/2,height/2,0),Color.blue);
        runMap.SetColor(new Vector3Int(width / 2-1, height / 2-1, 0), Color.blue);
    }
    
  
}
