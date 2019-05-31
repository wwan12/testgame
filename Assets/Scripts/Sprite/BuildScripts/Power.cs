using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    [Header("基础属性")]
    [Tooltip("是否是发电机")]
    public bool isAlternator;
    [Tooltip("电压V")]
    public float voltage;
    [Tooltip("电阻R")]
    public float resistance;
    [Tooltip("电流I(仅调试)")]
    [SerializeField]
    private float current;
    [Tooltip("输出功率")]
    public float outputPower;
    [Tooltip("额定功率")]
    public float ratedPower;
    /// <summary>
    /// 现在消耗的功率
    /// </summary>
    private float powerConsumption;
    /// <summary>
    /// 电源功率
    /// </summary>
    private float powerSupply;
    /// <summary>
    /// 内部储存的燃料（仅发电机）
    /// </summary>
    private float storage { get; set; }
    /// <summary>
    /// 连接的发电机（仅用电器）
    /// </summary>
    private GameObject source;
    [Header("连接属性")]
    [Tooltip("连接点（出）")]
    public Vector3 outConnectionPoint;
    [Tooltip("连接点（入）")]
    public Vector3 putConnectionPoint;
    [Tooltip("连接范围")]
    public float connectionScope=1f;
    [Tooltip("是否自动连接")]
    public bool isAutoConnection=true;
    

    // Start is called before the first frame update
    void Start()
    {
        outConnectionPoint += transform.position;
        putConnectionPoint += transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        powerConsumption = voltage * current;
    }

    public float GetEfficiency()
    {
        return powerConsumption / ratedPower;

    }

    public void Connection(Power p)
    {
        if (isAlternator&&p.isAlternator)
        {
            return;
        }
        else if(isAlternator)
        {
            if (p.source==null)
            {
                p.ConnectionSource(gameObject);
            }
        }
        else if(p.isAlternator)
        {
            if (source==null)
            {
                ConnectionSource(p.gameObject);
            }        
        }
    }

    public void ConnectionSource(GameObject s)
    {
        source = s;
    }

    public void DrawLine(Vector3 vector) {

    }

    public GameObject[] FindInScope(float scope)
    {
        GameObject[] powers= GameObject.FindGameObjectsWithTag("power");
        List<GameObject> ls = new List<GameObject>();
        foreach (var power in powers)
        {
            if ((power.transform.position - transform.position).sqrMagnitude <= scope * scope)
            {
                ls.Add(power);
            }
        }
        return ls.ToArray();
    }
    /// <summary>
    /// 自动连接
    /// </summary>
    public void AutoConnection()
    {
        GameObject[] powers= FindInScope(connectionScope);
        foreach (var p in powers)
        {
            p.GetComponent<Power>().Connection(this);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    public void BuildPlace(object obj,GameObject build) {
        if (build.tag.Equals("power"))
        {
            AutoConnection();
        }

    }

}
