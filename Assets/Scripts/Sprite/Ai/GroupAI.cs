using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 这是一个群组AI
/// </summary>
public class GroupAI : MonoBehaviour
{
   // static public GroupAI S;
    //配置参数，调整Boid对象的行为
    public int numBoids = 100;                  //boid 的个数
    public Boid boidPrefab;               //boid 在unity中的预制体
    public float spawnRadius = 100f;            //实例化 boid 的位置范围
    public float spawnVelcoty = 10f;            //boid 的速度
    public float minVelocity = 0f;
    public float maxVelocity = 30f;
    public float nearDist = 30f;                //判定为附近的 boid 的最小范围值
    public float collisionDist = 5f;            //判定为最近的 boid 的最小范围值(具有碰撞风险)
    public float velocityMatchingAmt = 0.01f;   //与 附近的boid 的平均速度 乘数(影响新速度)
    public float flockCenteringAmt = 0.15f;     //与 附近的boid 的平均三维间距 乘数(影响新速度)
    public float collisionAvoidanceAmt = -0.5f; //与 最近的boid 的平均三维间距 乘数(影响新速度)
    public float mouseAtrractionAmt = 0.01f;    //当 鼠标光标距离 过大时，与其间距的 乘数(影响新速度)
    public float mouseAvoidanceAmt = 0.75f;     //当 鼠标光标距离 过小时，与其间距的 乘数(影响新速度)
    public float mouseAvoiddanceDsit = 15f;
    public float velocityLerpAmt = 0.25f;       //线性插值法计算新速度的 乘数
    /// <summary>
    /// 目标
    /// </summary>
    public Vector3 mousePos;        //鼠标光标位置

    private void Start()
    {
        //设置单例变量S为BoidSpawner的当前实例
        //  S = this;

        //初始化NumBoids(当前为100)个Boids
        for (int i = 0; i < numBoids; i++) {
            boidPrefab.groupAI = this;
            Instantiate(boidPrefab.gameObject);
        }
            
    }

    private void LateUpdate()
    {
        //读取鼠标光标位置
       // Vector3 mousePos2d = new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.transform.position.y);

        //从世界空间到屏幕空间变换位置
        //mousePos = this.GetComponent<Camera>().ScreenToWorldPoint(mousePos2d);
    }
}



public class Boid : MonoBehaviour
{
    public GroupAI groupAI;

    public List<Boid> boids;     //实例化Boid 的表

    public Vector3 velocity;        //当前速度
    public Vector3 newVelocity;     //下一帧中的速度
    public Vector3 newPosition;     //下一帧中的位置

    public List<Boid> neighbors;        //附近所有的 Boid 的表
    public List<Boid> collisionRisks;   //距离过近的所有 Boid 的表(具有碰撞风险，需要处理)
    public Boid closest;                //最近的 Boid

    //初始化Boid
    private void Awake()
    {
        //如果List变量boids未定义，则对其进行定义
        if (boids == null)
            boids = new List<Boid>();

        //向Boids List 中添加Boid
        boids.Add(this);

        //为当前Boid实例提供一个随机的位置和速度
        //实例化的boid位置在 半径为 1*spawnRadius 的球形范围内
        Vector3 randPos = Random.insideUnitSphere * groupAI.spawnRadius;

        //只让Boid在xz平面上移动，并设定起始坐标
        randPos.y = 0;
        this.transform.position = randPos;

        //Random.onUnitSphere 返回 一个半径为1的 球体表面的点
        velocity = Random.onUnitSphere;
        velocity *= groupAI.spawnVelcoty;

        //初始化两个List
        neighbors = new List<Boid>();
        collisionRisks = new List<Boid>();

        //让this.transform成为Boid游戏对象的子对象
        this.transform.parent = GameObject.Find("Boids").transform;

        //给Boid设置一个随机的颜色
        Color randColor = Color.black;
        //设置颜色的颜色要 较深，非透明
        while (randColor.r + randColor.g + randColor.b < 1.0f)
            randColor = new Color(Random.value, Random.value, Random.value);
        //渲染 boid
        Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
            r.material.color = randColor;
    }

    private void Update()
    {
        //获取到 当前boid 附近所有的Boids 的表
        List<Boid> neighbors = GetNeighbors(this);

        //使用当前位置和速度初始化新位置和新速度
        newVelocity = velocity;
        newPosition = this.transform.position;

        //速度匹配
        //取得于 当前Boid 的速度接近的 所有邻近Boid对象 的平均速度
        Vector3 neighborVel = GetAverageVelocity(neighbors);
        //将 新速度 += 邻近boid的平均速度*velocityMatchingAmt
        newVelocity += neighborVel * groupAI.velocityMatchingAmt;

        /*
        凝聚向心性：使 当前boid 向 邻近Boid对象 的中心 移动
        */
        //取得于 当前Boid 的三位坐标接近的 所有邻近Boid对象 的平均三位间距
        Vector3 neighborCenterOffset = GetAveragePosition(neighbors) - this.transform.position;
        //将 新速度 += 邻近boid的平均间距*flockCenteringAmt
        newVelocity += neighborCenterOffset * groupAI.flockCenteringAmt;

        /*
        排斥性：避免撞到 邻近的Boid
        */
        Vector3 dist;
        if (collisionRisks.Count > 0)   //处理 最近的boid 表
        {
            //取得 最近的所有boid 的平均位置
            Vector3 collisionAveragePos = GetAveragePosition(collisionRisks);
            dist = collisionAveragePos - this.transform.position;
            //将 新速度 += 与最近boid的平均间距*flockCenteringAmt
            newVelocity += dist * groupAI.collisionAvoidanceAmt;
        }

        //跟随鼠标光标：无论距离多远都向鼠标光标移动
        dist = groupAI.mousePos - this.transform.position;

        //若距离鼠标光标太远，则靠近；反之离开(修改新速度)
        if (dist.magnitude > groupAI.mouseAvoiddanceDsit)
            newVelocity += dist * groupAI.mouseAtrractionAmt;
        else
            newVelocity -= dist.normalized * groupAI.mouseAvoidanceAmt;

        //至此在Update()内 确定了 新速度和新位置，需要在后续LateUpdate()内应用
        //一般都是Update()内确定参数，在LateUpdate()内实现移动
    }

    private void LateUpdate()
    {
        //使用线性插值法
        //基于计算出的新速度 进而修改 当前速度
        velocity = (1 - groupAI.velocityLerpAmt) * velocity + groupAI.velocityLerpAmt * newVelocity;

        //确保 速度值 在上下限范围内(超过范围就设定为范围值)
        if (velocity.magnitude > groupAI.maxVelocity)
            velocity = velocity.normalized * groupAI.maxVelocity;
        if (velocity.magnitude < groupAI.minVelocity)
            velocity = velocity.normalized * groupAI.minVelocity;

        //确定新位置(附加新方向)，相当于1s移动 velocity 的距离
        newPosition = this.transform.position + velocity * Time.deltaTime;

        //将所有对象限制在XZ平面
        //修改当前boid的方向：从原有位置看向新位置newPosition
        this.transform.LookAt(newPosition);

        //position移动方式，移动到新位置
        this.transform.position = newPosition;
    }

    //查找那些Boid距离当前Boid距离足够近，可以被当作附近对象
    public List<Boid> GetNeighbors(Boid boi)
    {
        float closesDist = float.MaxValue;  //最小间距，MaxValue 为浮点数的最大值
        Vector3 delta;              //当前 boid 与其他某个 boid 的三维间距 
        float dist;                 //三位间距转换为的 实数间距

        neighbors.Clear();          //清理上次表的数据
        collisionRisks.Clear();     //清理上次表的数据

        //遍历目前所有的 boid，依据设定的范围值筛选出 附近的boid 与 最近的boid 于各自表中
        foreach (Boid b in boids)
        {
            if (b == boi)   //跳过自身
                continue;

            delta = b.transform.position - boi.transform.position;  //遍历到的 b 与当前持有的 boi(都为boid) 的三维间距
            dist = delta.magnitude;     //实数间距

            if (dist < closesDist)
            {
                closesDist = dist;      //更新最小间距
                closest = b;            //更新最近的 boid 为 b
            }

            if (dist < groupAI.nearDist)  //处在附近的 boid 范围
                neighbors.Add(b);

            if (dist < groupAI.collisionDist) //处在最近的 boid 范围(有碰撞风险)
                collisionRisks.Add(b);
        }

        if (neighbors.Count == 0)   //若没有其他满足邻近范围的boid，则将自身boid纳入附近的boid表中
            neighbors.Add(closest);

        return (neighbors);
    }

    //获取 List<Boid>当中 所有Boid 的平均位置
    public Vector3 GetAveragePosition(List<Boid> someBoids)
    {
        Vector3 sum = Vector3.zero;
        foreach (Boid b in someBoids)
            sum += b.transform.position;
        Vector3 center = sum / someBoids.Count;

        return (center);
    }

    //获取 List<Boid> 当中 所有Boid 的平均速度
    public Vector3 GetAverageVelocity(List<Boid> someBoids)
    {
        Vector3 sum = Vector3.zero;
        foreach (Boid b in someBoids)
            sum += b.velocity;
        Vector3 avg = sum / someBoids.Count;

        return (avg);
    }
}
