using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

//todo 生成资源地图,摄像机视野到达边界时自动生成，与地块交互，与资源地块交互,兴趣点
public class MapManage : MonoBehaviour
{
    [Header("资源设置")]
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
    int[,][] map;
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
    public int width=8;
    [Tooltip("高")]
    public int height=8;
    [Tooltip("种子")]
    public int seed;
    [Tooltip("是否要使用随机种子")]
    public bool useRandomSeed;
    [Tooltip("是否使用覆盖式资源")]
    public bool isCover;
    [Tooltip("是否用墙包裹地图")]
    public bool isSurround;
    [Header("小地图设置")]
    [Tooltip("小地图，设置minimap图层")]
    public Tilemap miniMap;
    [Tooltip("代表墙的tile")]
    public Tile miniWallTile;
    [Tooltip("代表道路的tile")]
    public Tile miniRunTile;

    private bool isExhausted;
    private System.Random pseudoRandom;
    private System.Random pseudoRandomRes;
    //private bool hasPlayer;
    //private GameObject player;
    //Interest point


    // Start is called before the first frame update
    void Start()
    {
        map = new int[width, height][];
       // player = GameObject.FindGameObjectWithTag("Player");
        CreateMap();
      //  ChangeTile();
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < width; i++)//监控资源余量为0时销毁对应资源地块
        {
            for (int j = 0; j < height; j++)
            {
                if (map[i, j][2] != 0 && map[i, j][4] == 0)
                {
                    map[i, j][2] = 0;
                    resMap.SetTile(new Vector3Int(j - width / 2, i - height / 2, 0), null);
                }
            }
        }
    
        
        //if (!hasPlayer&&Input.GetMouseButtonDown(0))
        //{           
        //   Vector3 vector3= GetClickPosition(runMap);
        //   player.GetComponent<PlayerManage>().CreatePlayerInMap(vector3);
        //   hasPlayer = true;
        //}
    }

    void GenerateMap()
    {
        RandomFillMap();    //随机生成地图
       // RandomFillResMap();
        for (int i = 0; i < 3; i++)
        {
            SmoothMap(map,0);
            SmoothMap(map, 2);
        }
        DrawMap();
        DrawMiniMap();
    }
    /// <summary>
    /// 第一次填充地图
    /// </summary>
    void RandomFillMap()
    {
        if (useRandomSeed)
            seed = DateTime.Now.ToString().GetHashCode();

        pseudoRandom = new System.Random(seed);
        pseudoRandomRes=new System.Random(seed+99);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                map[i, j] = new int[saveMapLength];
                if (isSurround&&(i == 0 || i == width - 1 || j == 0 || j == height - 1))
                {
                    //边缘是墙
                    map[i, j][0] = 1;
                }
                else {
                   
                    if (width/2-2<i&&i<width/2+2&&height/2-2<j&&j<height/2+2)//中间空出3*3的格子
                    {
                        map[i, j][0] =  0;
                    }
                    else
                    {
                        map[i, j][0] = (pseudoRandom.Next(0, 100) < probability) ? 1 : 0;  //1是墙，0是空地
                        map[i, j][2] = (pseudoRandomRes.Next(0, 100) < resourceDensity) ? 1 : 0;         
                        map[i, j][4] = (int)Rand(resourceAbundance, resourceAbundance/4);//第一次生成
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
                    if (map[i, j][0] == 0)//画出道路和墙,从0，0开始画
                    {
                        runMap.SetTile(new Vector3Int(j-width/2, i-height/2, 0), baseTile[map[i,j][1]] );
                        if (map[i,j][2]==1)
                        {
                            resMap.SetTile(new Vector3Int(j - width / 2, i - height / 2, 0), resourcesTiles[map[i, j][3]]);
                        }                     
                    }
                    else
                    {
                        wallMap.SetTile(new Vector3Int(j - width / 2, i - height / 2, 0), wallTile);
                    }
                }
            }
        }
        BakeMap();
    }

    void DrawMiniMap()
    {
        if (miniMap==null)
        {
            return;
        }
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (map[i, j][0] == 0)//画出道路和墙
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

    void BakeMap() {
        runMap.GetComponent<NavMeshSurface>().BuildNavMesh();
        


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
        float deviation = width/10;
        boundary.GetComponent<PolygonCollider2D>().points = new Vector2[] { new Vector2(-width / 2 + deviation, height / 2 - deviation), new Vector2(-width / 2 + deviation, -height / 2 + deviation), new Vector2(width / 2 - deviation, -height / 2 + deviation), new Vector2(width / 2 - deviation, height / 2 - deviation) };
    }

    /// <summary>
    /// 扩充地图
    /// </summary>
    /// <param name="extra"></param>
    public void ExpandMap(int extra)
    {
        width += extra;
        height += extra;
        int[,][] expand= new int[width, height][];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                expand[i, j] = new int[saveMapLength];
                if (extra < i && i < width  && extra < j && j < height )//已加载的地图数据不变
                {
                    expand[i, j][0] = 0;
                }
                else
                {
                    expand[i, j][0] = (pseudoRandom.Next(0, 100) < probability) ? 1 : 0;  //1是墙，0是空地
                }

            }
        }
        for (int i = 0; i < 2; i++)
        {
            SmoothMap(expand,0);
            SmoothMap(expand,2);
        }
        for (int i = 0; i < width-extra; i++)
        {
            for (int j = 0; j < height-extra; j++)
            {
                expand[i + extra, j + extra] = map[i, j];
            }
        }
        map = expand;     
        DrawMap();
        DrawMiniMap();
        ChangeBoundary();
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
                for (int c = 0; c < map[i, j].Length; c++)
                {
                    saveMapData.Append(map[i,j][c]);
                    if (!(c== map[i, j].Length-1))
                    {
                        saveMapData.Append(",");
                    }
                }
            
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
            string[] tileData= tileDatas[i].Split(',');
            int x = int.Parse(tileData[0]);
            int y = int.Parse(tileData[1]);
            map[x, y] = new int[saveMapLength];
            for (int j = 2; j < tileData.Length; j++)
            {  
               map[x,y][j]=int.Parse(tileData[j]);           
            }       
        }
        DrawMap();
        DrawMiniMap();
    }


//**去噪点
    int GetSurroundingWalls(int posX, int posY,int index)
    {
        int wallCount = 0;

        for (int i = posX - 1; i <= posX + 1; i++)
        {
            for (int j = posY - 1; j <= posY + 1; j++)
            {
                if (i >= 0 && i < width && j >= 0 && j < height)
                {
                    if (i != posX || j != posY)
                        wallCount += map[i, j][index];
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }
    /// <summary>
    /// 将随机生成的数据集群化
    /// </summary>
    void SmoothMap(int[,][] data,int index)
    {
        //data.GetLength(0);
        //data.GetLength(1);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int surroundingTiles = GetSurroundingWalls(i, j, index);

                if (surroundingTiles > 4)
                    data[i, j][index] = 1;
                else if (surroundingTiles < 4)
                    data[i, j][index] = 0;
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
